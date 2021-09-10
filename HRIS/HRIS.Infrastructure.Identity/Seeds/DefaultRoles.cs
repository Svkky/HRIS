using HRIS.Application.Enums;
using HRIS.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Audit.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Accountant.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Cashier.ToString()));
        }
    }
}
