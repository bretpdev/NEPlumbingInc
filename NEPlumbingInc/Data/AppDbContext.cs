public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AdminUser> AdminUsers { get; set; }

    public DbSet<ServicesFormModel> Services { get; set; }

    public DbSet<UndergroundSubmission> UndergroundSubmissions { get; set; }

}