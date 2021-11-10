using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary.Models
{
    public partial class IdentDbContext : IdentityDbContext<ApplicationUsers, ApplicationRoles, Guid,
        IdentityUserClaim<Guid>, ApplicationUserRoles, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        public IdentDbContext()
        {
        }

        public IdentDbContext(DbContextOptions<IdentDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationRoles> ApplicationRoles { get; set; }
        public virtual DbSet<ApplicationUsers> ApplicationUsers { get; set; }
        public virtual DbSet<IdentityRoleClaim<Guid>> ApplicationRoleClaims { get; set; }
        public virtual DbSet<IdentityUserClaim<Guid>> ApplicationUserClaims { get; set; }
        public virtual DbSet<IdentityUserLogin<Guid>> ApplicationUserLogins { get; set; }
        public virtual DbSet<ApplicationUserRoles> ApplicationUserRoles { get; set; }
        public virtual DbSet<IdentityUserToken<Guid>> ApplicationUserToken { get; set; }
        public virtual DbSet<OpenIddictApplications> OpenIddictApplications { get; set; }
        public virtual DbSet<OpenIddictAuthorizations> OpenIddictAuthorizations { get; set; }
        public virtual DbSet<OpenIddictScopes> OpenIddictScopes { get; set; }
        public virtual DbSet<OpenIddictTokens> OpenIddictTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseOpenIddict();

            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("ApplicationRoleClaims");
            });

            modelBuilder.Entity<ApplicationRoles>(entity =>
            {
                entity.ToTable("ApplicationRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserLogins");

                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<ApplicationUserRoles>(entity =>
            {
                entity.ToTable("ApplicationUserRoles");

                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserTokens");

                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<ApplicationUsers>(entity =>
            {
                entity.ToTable("ApplicationUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
