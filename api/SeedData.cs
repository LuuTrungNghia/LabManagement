using Microsoft.AspNetCore.Identity;
using api.Models;

namespace api
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider services, UserManager<ApplicationUser> userManager)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "admin", "user", "lecturer", "student" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var user = await userManager.FindByEmailAsync("admin@example.com");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FullName = "Admin User",
                    Avatar = "default-avatar.png",
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    Gender = "Male"
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
        }
    }
}
