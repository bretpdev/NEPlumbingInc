public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, AppDbContext context)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<SeedData>>();
        logger.LogInformation("Starting database initialization");

        await context.Database.EnsureCreatedAsync();
        var user = await context.AdminUsers.AnyAsync();
        if (!user)
        {
            logger.LogInformation("No users found, seeding admin user");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("bret6532");
            context.AdminUsers.Add(new AdminUser
            {
                Username = "bret",
                PasswordHash = hashedPassword
            });
            await context.SaveChangesAsync();
            logger.LogInformation("Admin user seeded successfully");
        }
    }
}