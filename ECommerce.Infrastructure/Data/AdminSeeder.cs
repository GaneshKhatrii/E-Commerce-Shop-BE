using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(ApplicationDbContext context)
        {
            // Check if admin already exists
            var adminExists = await context.Users.AnyAsync(user => user.Role == UserRole.Admin);

            if (adminExists)
            {
                return;
            }

            var admin = new User()
            {
                FirstName = "System",
                LastName = "Admin",
                Email = "admin@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                PhoneNumber = "1234567899",
                Role = UserRole.Admin,
                IsEmailVerified = true
            };

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}
