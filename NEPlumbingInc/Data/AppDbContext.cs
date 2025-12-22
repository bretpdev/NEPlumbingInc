using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<IdentityUser>
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
    public DbSet<SubServiceModel> SubServices { get; set; }
    public DbSet<HomePageContent> HomePageContents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the relationship between ServicesFormModel and SubServiceModel
        modelBuilder.Entity<ServicesFormModel>()
            .HasMany(s => s.SubServices)
            .WithOne(s => s.Service)
            .HasForeignKey(s => s.ServiceId);
    }
}