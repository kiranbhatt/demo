using Books.API.Entities;
using Books.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Books.Core.Seeds
{
    public enum Roles
    {
        SuperAdmin,
        Admin,
        User
    }

    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.SuperAdmin.ToString()))
            {
                await roleManager.CreateAsync(new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = Roles.SuperAdmin.ToString() });
            }
            if (!await roleManager.RoleExistsAsync(Roles.Admin.ToString()))
            {
                await roleManager.CreateAsync(new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = Roles.Admin.ToString() });
            }
            if (!await roleManager.RoleExistsAsync(Roles.User.ToString()))
            {
                await roleManager.CreateAsync(new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = Roles.User.ToString() });
            } 
        }
    }
}
