public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<LoginViewModel> LoginUsers { get; set; }

    public DbSet<ServicesFormModel> Services { get; set; }

    public DbSet<UndergroundSubmission> UndergroundSubmissions { get; set; }

    public DbSet<SpecialOffer> SpecialOffers { get; set; }

}