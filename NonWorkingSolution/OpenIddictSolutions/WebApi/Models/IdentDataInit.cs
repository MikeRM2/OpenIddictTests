using DataLibrary.IdentityManagers;
using DataLibrary.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    // This class does not exist in my project.  Created for purposes of testing.
    public static class IdentDataInit
    {
        public static void SeedData(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(ApplicationUserManager userManager)
        {
            if (userManager.FindByNameAsync("TestUser1@localhost.com").Result == null)
            {
                ApplicationUsers user = new ApplicationUsers();
                user.UserName = "TestUser1@localhost.com";
                user.Email = "TestUser1@localhost.com";
                user.FirstName = "Site";
                user.LastName = "User";
                user.IsActive = true;
                user.EmailConfirmed = true;

                IdentityResult userResult = userManager.CreateAsync(user, "P@ssW0rd").Result;

                if (userResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "SiteUser");
                }
            }

            if (userManager.FindByNameAsync("TestUser2@localhost.com").Result == null)
            {
                ApplicationUsers user = new ApplicationUsers();
                user.UserName = "TestUser2@localhost.com";
                user.Email = "TestUser2@localhost.com";
                user.FirstName = "Site";
                user.LastName = "Admin";
                user.IsActive = true;
                user.EmailConfirmed = true;

                IdentityResult userResult = userManager.CreateAsync(user, "P@ssW0rd1").Result;

                if (userResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "SiteAdmin");
                }
            }
        }

        public static void SeedRoles(ApplicationRoleManager roleManager)
        {
            if (!roleManager.RoleExistsAsync("SiteUser").Result)
            {
                ApplicationRoles role = new ApplicationRoles();
                role.Name = "SiteUser";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("SiteAdmin").Result)
            {
                ApplicationRoles role = new ApplicationRoles();
                role.Name = "SiteAdmin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
