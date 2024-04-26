using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SavorySeasons.Models;


namespace SavorySeasons.SeedData
{
    public class SeedRolesAndUsers
    {
        public static async Task SeedUsers(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<IdentityRole>
            {
                new IdentityRole {Name= "Admin" },
                new IdentityRole {Name= "User" },
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new ApplicationUser
            {
                Email = "savoryseasons.admin@gmail.com",
                UserName = "savoryseasons.admin@gmail.com",
                Name="Null",   
            };

            await userManager.CreateAsync(admin, "$Team@123");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
