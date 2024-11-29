using Microsoft.AspNetCore.Identity;
using api.Models;
using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider services, UserManager<ApplicationUser> userManager)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "admin", "user", "student", "lecturer" };

            // Create roles if they don't exist
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user if it doesn't exist
            var admin = await userManager.FindByEmailAsync("admin@example.com");
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com", // Admin user needs an email
                    FullName = "Administrator",
                    Avatar = "default-avatar.png",
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    Gender = "Male",
                    IsApproved = true  // Ensure the account is approved
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }

            // Create server user (without email)
            var serverUser = await userManager.FindByNameAsync("server");
            if (serverUser == null)
            {
                serverUser = new ApplicationUser
                {
                    UserName = "server", // No email for server user
                    IsApproved = true  // Server accounts are auto-approved
                };

                var result = await userManager.CreateAsync(serverUser, "Server@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(serverUser, "admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }

            // Initialize lab rooms if not already created
            using (var context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();
                if (!context.Labs.Any())
                {
                    context.Labs.Add(new Lab
                    {
                        LabName = "Phòng Lab 301",
                        Description = "Phòng lab mặc định",
                        IsAvailable = true
                    });
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
