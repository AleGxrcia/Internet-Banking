using InternetBanking.Core.Application.Enums;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Client.ToString()));
        }
    }
}
