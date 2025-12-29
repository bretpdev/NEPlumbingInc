public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, AppDbContext context)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<SeedData>>();
        logger.LogInformation("Starting database initialization");

        await context.Database.EnsureCreatedAsync();
        var user = await context.LoginUsers.AnyAsync();
        if (!user)
        {
            logger.LogInformation("No users found, seeding admin user");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
            context.LoginUsers.Add(new LoginViewModel
            {
                UserName = "admin",
                Password = hashedPassword
            });
            await context.SaveChangesAsync();
            logger.LogInformation("Admin user seeded successfully");
        }
    }
}