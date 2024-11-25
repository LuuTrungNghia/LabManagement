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

            // Tạo các vai trò nếu chưa tồn tại
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Tạo người dùng admin nếu chưa tồn tại
            var admin = await userManager.FindByEmailAsync("admin@example.com");
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FullName = "Administrator",
                    Avatar = "default-avatar.png",
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    Gender = "Male"
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            // Khởi tạo phòng lab nếu chưa có
            using (var context = new ApplicationDbContext(
                services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
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
