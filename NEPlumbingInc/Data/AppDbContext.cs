public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<LoginViewModel> LoginUsers { get; set; }

    public DbSet<ServicesFormModel> Services { get; set; }

    public DbSet<UndergroundSubmission> UndergroundSubmissions { get; set; }

    public DbSet<SpecialOffer> SpecialOffers { get; set; }

    public DbSet<MessageViewModel> Messages { get; set; }
}