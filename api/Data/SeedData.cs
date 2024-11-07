using Microsoft.AspNetCore.Identity;
using api.Models;
using System;
using System.Threading.Tasks;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Create the "Admin" role if it doesn't exist.
        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole == null)
        {
            adminRole = new IdentityRole("Admin");
            await roleManager.CreateAsync(adminRole);
        }

        // Create the "Admin" user if it doesn't exist.
        var existingAdmin = await userManager.FindByEmailAsync("admin@example.com");
        if (existingAdmin == null)
        {
            var user = new User
            {
                Email = "admin@example.com",
                UserName = "admin@example.com", // Identity uses this as the default identifier
                Name = "Admin",                 // Assuming your User model has a "Name" field
                IsApproved = true               // Assuming your User model has an "IsApproved" field
            };

            var result = await userManager.CreateAsync(user, "Admin@123"); // Ensure password meets policy
            if (result.Succeeded)
            {
                // Assign the user to the "Admin" role
                await userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                // You may want to log the errors or handle them here if needed.
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                }
            }
        }
    }
}
