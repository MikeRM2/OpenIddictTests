using DataLibrary.IdentityManagers;
using DataLibrary.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Validation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();

            services.AddDbContext<IdentDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("IdentityDB"));

                options.UseOpenIddict();
            });

            // Add the Identity Services we are going to be using the Application Users and the Application Roles
            services.AddIdentity<ApplicationUsers, ApplicationRoles>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.SignIn.RequireConfirmedAccount = true;
                config.User.RequireUniqueEmail = true;
                config.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<IdentDbContext>()
            .AddRoleManager<ApplicationRoleManager>()
            .AddUserManager<ApplicationUserManager>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            services.AddOpenIddict()
                // Register the OpenIddict core components.
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the Entity Framework Core stores and models.
                    // Note: call ReplaceDefaultEntities() to replace the default entities.
                    options.UseEntityFrameworkCore()
                    .UseDbContext<IdentDbContext>();
                })
                // Register the OpenIddict server components.
                .AddServer(options =>
                {                    
                    options.SetAuthorizationEndpointUris("/Authorize")
                    .SetTokenEndpointUris("/Token");

                    // Mark the "OpenId", "email", "profile" and "roles" scopes as supported scopes.
                    options.RegisterScopes(Scopes.OpenId, Scopes.Email, Scopes.Profile, Scopes.Roles, "DatabaseName");

                    options.SetIssuer(new Uri(Configuration["Jwt:Issuer"]));


                    // Enable the available flows.  Which flow do I need?
                    options.AllowAuthorizationCodeFlow()
                    .RequireProofKeyForCodeExchange();

                    //options.AcceptAnonymousClients();

                    //options.DisableAccessTokenEncryption();

                    // Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate()
                          .AddDevelopmentSigningCertificate();

                    // Register the ASP.NET Core host and configure the ASP.NET Core options.
                    options.UseAspNetCore()
                           .EnableAuthorizationEndpointPassthrough()
                           .EnableTokenEndpointPassthrough();
                })
                // Register the OpenIddict validation components.
                .AddValidation(options =>
                {
                    // Import the configuration from the local OpenIddict server instance.
                    options.UseLocalServer();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });

            services.AddHostedService<TestData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // The ApplicationRoleManager and ApplicationUserManager do not exist in original startup class.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationRoleManager roleManager, ApplicationUserManager userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            IdentDataInit.SeedData(userManager, roleManager);  // This is not in the original startup class

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
