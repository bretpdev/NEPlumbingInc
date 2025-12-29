public class HomePageContentService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public HomePageContentService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<HomePageContent> GetContentAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var content = await context.HomePageContents.FirstOrDefaultAsync();
        if (content == null)
        {
            content = new HomePageContent
            {
                HeroBadgeText = "Serving Norther Utah and Park City Since 2004",
                HeroTitle = "Elevated Plumbing for Elevated Living",
                HeroDescription = "From heated driveways to tankless water systems and high-end fixtures, we deliver unmatched craftsmanship to the most discerning homeowners.",
                FeatureBadge1 = "Licensed & Insured",
                FeatureBadge2 = "Award-Winning Service",
                FeatureBadge3 = "24/7 Emergency Service",
                PremiumServicesTitle = "Premium Plumbing Services",
                Service1Title = "Luxury Fixtures",
                Service1Description = "High-end bathroom and kitchen fixtures installed with precision and care.",
                Service2Title = "Heated Driveways",
                Service2Description = "Specialized snow-melting systems for driveways and walkways.",
                Service3Title = "On-Demand Water Heaters",
                Service3Description = "Efficient tankless water heaters for continuous, energy-saving hot water.",
                UpdatedAt = DateTime.Now
            };
            context.HomePageContents.Add(content);
            await context.SaveChangesAsync();
        }
        else
        {
            // Fill in any null fields with defaults
            bool needsUpdate = false;
            if (string.IsNullOrEmpty(content.HeroBadgeText))
            {
                content.HeroBadgeText = "Serving Norther Utah and Park City Since 2004";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.HeroTitle))
            {
                content.HeroTitle = "Elevated Plumbing for Elevated Living";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.HeroDescription))
            {
                content.HeroDescription = "From heated driveways to tankless water systems and high-end fixtures, we deliver unmatched craftsmanship to the most discerning homeowners.";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.FeatureBadge1))
            {
                content.FeatureBadge1 = "Licensed & Insured";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.FeatureBadge2))
            {
                content.FeatureBadge2 = "Award-Winning Service";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.FeatureBadge3))
            {
                content.FeatureBadge3 = "24/7 Emergency Service";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.PremiumServicesTitle))
            {
                content.PremiumServicesTitle = "Premium Plumbing Services";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.Service1Title))
            {
                content.Service1Title = "Luxury Fixtures";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.Service1Description))
            {
                content.Service1Description = "High-end bathroom and kitchen fixtures installed with precision and care.";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.Service2Title))
            {
                content.Service2Title = "Heated Driveways";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.Service2Description))
            {
                content.Service2Description = "Specialized snow-melting systems for driveways and walkways.";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.Service3Title))
            {
                content.Service3Title = "On-Demand Water Heaters";
                needsUpdate = true;
            }
            if (string.IsNullOrEmpty(content.Service3Description))
            {
                content.Service3Description = "Efficient tankless water heaters for continuous, energy-saving hot water.";
                needsUpdate = true;
            }

            if (needsUpdate)
            {
                content.UpdatedAt = DateTime.Now;
                context.HomePageContents.Update(content);
                await context.SaveChangesAsync();
            }
        }
        return content;
    }

    public async Task UpdateContentAsync(HomePageContent content)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        content.UpdatedAt = DateTime.Now;
        context.HomePageContents.Update(content);
        await context.SaveChangesAsync();
    }
}
