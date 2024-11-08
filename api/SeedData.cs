using Microsoft.AspNetCore.Identity;
using api.Models;

namespace api
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider services, UserManager<ApplicationUser> userManager)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Create roles if they do not exist
            string[] roleNames = { "admin", "user" };  // Add any other roles as needed
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Check if the admin user exists
            var user = await userManager.FindByEmailAsync("admin@example.com");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FullName = "Admin User",
                    Avatar = "default-avatar.png",
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    Gender = "Male"
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");  // Add user to the admin role
                }
            }
        }
    }
}
