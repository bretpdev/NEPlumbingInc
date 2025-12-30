namespace NEPlumbingInc.Services;

public interface ICareersPageSettingsService
{
    Task<CareersPageSettings> GetSettingsAsync();
    Task UpdateSettingsAsync(CareersPageSettings settings);
}

public class CareersPageSettingsService(IDbContextFactory<AppDbContext> contextFactory) : ICareersPageSettingsService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public async Task<CareersPageSettings> GetSettingsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var settings = await context.CareersPageSettings.FirstOrDefaultAsync();

        if (settings == null)
        {
            settings = new CareersPageSettings();
            context.CareersPageSettings.Add(settings);
            await context.SaveChangesAsync();
            return settings;
        }

        var needsUpdate = false;

        if (string.IsNullOrWhiteSpace(settings.LookingForText))
        {
            settings.LookingForText = new CareersPageSettings().LookingForText;
            needsUpdate = true;
        }

        if (string.IsNullOrWhiteSpace(settings.HelpfulToIncludeText))
        {
            settings.HelpfulToIncludeText = new CareersPageSettings().HelpfulToIncludeText;
            needsUpdate = true;
        }

        if (needsUpdate)
        {
            settings.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        return settings;
    }

    public async Task UpdateSettingsAsync(CareersPageSettings settings)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));

        using var context = await _contextFactory.CreateDbContextAsync();
        var existing = await context.CareersPageSettings.FirstOrDefaultAsync();

        if (existing == null)
        {
            settings.UpdatedAt = DateTime.UtcNow;
            context.CareersPageSettings.Add(settings);
        }
        else
        {
            existing.IsHiringEnabled = settings.IsHiringEnabled;
            existing.LookingForText = settings.LookingForText ?? string.Empty;
            existing.HelpfulToIncludeText = settings.HelpfulToIncludeText ?? string.Empty;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
    }
}
