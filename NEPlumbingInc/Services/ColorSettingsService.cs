namespace NEPlumbingInc.Services;

public interface IColorSettingsService
{
    Task<ColorSettings> GetColorSettingsAsync();
    Task<ColorSettings> UpdateColorSettingsAsync(ColorSettings settings);
    Task ResetToDefaultAsync();
}

public class ColorSettingsService(IDbContextFactory<AppDbContext> contextFactory) : IColorSettingsService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public async Task<ColorSettings> GetColorSettingsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var settings = await context.ColorSettings.FirstOrDefaultAsync();
        
        if (settings == null)
        {
            settings = new ColorSettings
            {
                PrimaryColor = "#0066CC",
                SecondaryColor = "#005299",
                AccentColor = "#003d7a",
                TextColor = "#212529",
                LightBgColor = "#f8f9fa",
                UpdatedAt = DateTime.UtcNow
            };
            context.ColorSettings.Add(settings);
            await context.SaveChangesAsync();
        }
        
        return settings;
    }

    public async Task<ColorSettings> UpdateColorSettingsAsync(ColorSettings settings)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var existing = await context.ColorSettings.FirstOrDefaultAsync();
        
        if (existing != null)
        {
            existing.PrimaryColor = settings.PrimaryColor;
            existing.SecondaryColor = settings.SecondaryColor;
            existing.AccentColor = settings.AccentColor;
            existing.TextColor = settings.TextColor;
            existing.LightBgColor = settings.LightBgColor;
            existing.UpdatedAt = DateTime.UtcNow;
            
            context.ColorSettings.Update(existing);
        }
        else
        {
            settings.UpdatedAt = DateTime.UtcNow;
            context.ColorSettings.Add(settings);
        }
        
        await context.SaveChangesAsync();
        return existing ?? settings;
    }

    public async Task ResetToDefaultAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var existing = await context.ColorSettings.FirstOrDefaultAsync();
        
        if (existing != null)
        {
            existing.PrimaryColor = "#0066CC";
            existing.SecondaryColor = "#005299";
            existing.AccentColor = "#003d7a";
            existing.TextColor = "#212529";
            existing.LightBgColor = "#f8f9fa";
            existing.UpdatedAt = DateTime.UtcNow;
            
            context.ColorSettings.Update(existing);
            await context.SaveChangesAsync();
        }
    }
}
