public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AdminUser> AdminUsers { get; set; }

    public DbSet<ServicesFormModel> Services { get; set; }
}