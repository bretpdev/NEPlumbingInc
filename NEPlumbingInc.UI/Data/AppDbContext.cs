public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AdminUser> AdminUsers { get; set; }

    public DbSet<Services> Services { get; set; }

    public DbSet<Project> Projects { get; set; }
}