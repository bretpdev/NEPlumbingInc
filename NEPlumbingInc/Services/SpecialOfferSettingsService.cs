namespace NEPlumbingInc.Services;

public interface ISpecialOfferSettingsService
{
    Task<SpecialOfferSettings> GetSettingsAsync();
    Task UpdateSettingsAsync(SpecialOfferSettings settings);
    Task ResetToDefaultAsync();
}

public class SpecialOfferSettingsService(AppDbContext context) : ISpecialOfferSettingsService
{
    private readonly AppDbContext _context = context;

    public async Task<SpecialOfferSettings> GetSettingsAsync()
    {
        var settings = await _context.SpecialOfferSettings.FirstOrDefaultAsync();
        
        if (settings == null)
        {
            // Create default settings if they don't exist
            settings = new SpecialOfferSettings();
            _context.SpecialOfferSettings.Add(settings);
            await _context.SaveChangesAsync();
        }

        return settings;
    }

    public async Task UpdateSettingsAsync(SpecialOfferSettings settings)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));

        var existingSettings = await _context.SpecialOfferSettings.FirstOrDefaultAsync();
        
        if (existingSettings == null)
        {
            settings.UpdatedAt = DateTime.UtcNow;
            _context.SpecialOfferSettings.Add(settings);
        }
        else
        {
            existingSettings.IsEnabled = settings.IsEnabled;
            existingSettings.HeadingText = settings.HeadingText;
            existingSettings.DescriptionText = settings.DescriptionText;
            existingSettings.ButtonText = settings.ButtonText;
            existingSettings.NoOfferHeading = settings.NoOfferHeading;
            existingSettings.NoOfferDescription = settings.NoOfferDescription;
            existingSettings.NewsletterText = settings.NewsletterText;
            existingSettings.MaxOffersLimit = settings.MaxOffersLimit;
            existingSettings.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task ResetToDefaultAsync()
    {
        var existingSettings = await _context.SpecialOfferSettings.FirstOrDefaultAsync();
        
        if (existingSettings != null)
        {
            _context.SpecialOfferSettings.Remove(existingSettings);
            await _context.SaveChangesAsync();
        }

        var defaultSettings = new SpecialOfferSettings();
        _context.SpecialOfferSettings.Add(defaultSettings);
        await _context.SaveChangesAsync();
    }
}
