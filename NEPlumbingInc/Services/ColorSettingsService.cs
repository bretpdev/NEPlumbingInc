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

    private static readonly ColorSettings DefaultSettings = new()
    {
        // Light
        PrimaryColor = "#0066CC",
        SecondaryColor = "#005299",
        AccentColor = "#003d7a",
        TextColor = "#212529",
        LightBgColor = "#f8f9fa",
        HeroBadgeColor = "#0056b3",
        ButtonColor = "#0066CC",
        SurfaceColor = "#ffffff",
        SurfaceAltColor = "#f8f9fa",
        InputBgColor = "#ffffff",
        BorderColor = "#dee2e6",

        // Dark
        DarkPrimaryColor = "#569cd6",
        DarkSecondaryColor = "#4f8cc4",
        DarkAccentColor = "#9cdcfe",
        DarkTextColor = "#d4d4d4",
        DarkBgColor = "#1e1e1e",
        DarkHeroBadgeColor = "#2d2d30",
        DarkButtonColor = "#569cd6",
        DarkSurfaceColor = "#252526",
        DarkSurfaceAltColor = "#2d2d30",
        DarkInputBgColor = "#3c3c3c",
        DarkBorderColor = "#3e3e42",
    };

    public async Task<ColorSettings> GetColorSettingsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var settings = await context.ColorSettings.FirstOrDefaultAsync();
        
        if (settings == null)
        {
            settings = new ColorSettings
            {
                PrimaryColor = DefaultSettings.PrimaryColor,
                SecondaryColor = DefaultSettings.SecondaryColor,
                AccentColor = DefaultSettings.AccentColor,
                TextColor = DefaultSettings.TextColor,
                LightBgColor = DefaultSettings.LightBgColor,
                HeroBadgeColor = DefaultSettings.HeroBadgeColor,
                ButtonColor = DefaultSettings.ButtonColor,
                SurfaceColor = DefaultSettings.SurfaceColor,
                SurfaceAltColor = DefaultSettings.SurfaceAltColor,
                InputBgColor = DefaultSettings.InputBgColor,
                BorderColor = DefaultSettings.BorderColor,
                DarkPrimaryColor = DefaultSettings.DarkPrimaryColor,
                DarkSecondaryColor = DefaultSettings.DarkSecondaryColor,
                DarkAccentColor = DefaultSettings.DarkAccentColor,
                DarkTextColor = DefaultSettings.DarkTextColor,
                DarkBgColor = DefaultSettings.DarkBgColor,
                DarkHeroBadgeColor = DefaultSettings.DarkHeroBadgeColor,
                DarkButtonColor = DefaultSettings.DarkButtonColor,
                DarkSurfaceColor = DefaultSettings.DarkSurfaceColor,
                DarkSurfaceAltColor = DefaultSettings.DarkSurfaceAltColor,
                DarkInputBgColor = DefaultSettings.DarkInputBgColor,
                DarkBorderColor = DefaultSettings.DarkBorderColor,
                UpdatedAt = DateTime.UtcNow
            };
            context.ColorSettings.Add(settings);
            await context.SaveChangesAsync();
        }
        else
        {
            // Backfill any missing/blank values (defensive for older rows or partial deployments)
            bool changed = false;

            settings.PrimaryColor = CoalesceColor(settings.PrimaryColor, DefaultSettings.PrimaryColor, ref changed);
            settings.SecondaryColor = CoalesceColor(settings.SecondaryColor, DefaultSettings.SecondaryColor, ref changed);
            settings.AccentColor = CoalesceColor(settings.AccentColor, DefaultSettings.AccentColor, ref changed);
            settings.TextColor = CoalesceColor(settings.TextColor, DefaultSettings.TextColor, ref changed);
            settings.LightBgColor = CoalesceColor(settings.LightBgColor, DefaultSettings.LightBgColor, ref changed);
            settings.HeroBadgeColor = CoalesceColor(settings.HeroBadgeColor, DefaultSettings.HeroBadgeColor, ref changed);
            settings.ButtonColor = CoalesceColor(settings.ButtonColor, DefaultSettings.ButtonColor, ref changed);
            settings.SurfaceColor = CoalesceColor(settings.SurfaceColor, DefaultSettings.SurfaceColor, ref changed);
            settings.SurfaceAltColor = CoalesceColor(settings.SurfaceAltColor, DefaultSettings.SurfaceAltColor, ref changed);
            settings.InputBgColor = CoalesceColor(settings.InputBgColor, DefaultSettings.InputBgColor, ref changed);
            settings.BorderColor = CoalesceColor(settings.BorderColor, DefaultSettings.BorderColor, ref changed);

            settings.DarkPrimaryColor = CoalesceColor(settings.DarkPrimaryColor, DefaultSettings.DarkPrimaryColor, ref changed);
            settings.DarkSecondaryColor = CoalesceColor(settings.DarkSecondaryColor, DefaultSettings.DarkSecondaryColor, ref changed);
            settings.DarkAccentColor = CoalesceColor(settings.DarkAccentColor, DefaultSettings.DarkAccentColor, ref changed);
            settings.DarkTextColor = CoalesceColor(settings.DarkTextColor, DefaultSettings.DarkTextColor, ref changed);
            settings.DarkBgColor = CoalesceColor(settings.DarkBgColor, DefaultSettings.DarkBgColor, ref changed);
            settings.DarkHeroBadgeColor = CoalesceColor(settings.DarkHeroBadgeColor, DefaultSettings.DarkHeroBadgeColor, ref changed);
            settings.DarkButtonColor = CoalesceColor(settings.DarkButtonColor, DefaultSettings.DarkButtonColor, ref changed);
            settings.DarkSurfaceColor = CoalesceColor(settings.DarkSurfaceColor, DefaultSettings.DarkSurfaceColor, ref changed);
            settings.DarkSurfaceAltColor = CoalesceColor(settings.DarkSurfaceAltColor, DefaultSettings.DarkSurfaceAltColor, ref changed);
            settings.DarkInputBgColor = CoalesceColor(settings.DarkInputBgColor, DefaultSettings.DarkInputBgColor, ref changed);
            settings.DarkBorderColor = CoalesceColor(settings.DarkBorderColor, DefaultSettings.DarkBorderColor, ref changed);

            if (changed)
            {
                settings.UpdatedAt = DateTime.UtcNow;
                context.ColorSettings.Update(settings);
                await context.SaveChangesAsync();
            }
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
            existing.HeroBadgeColor = settings.HeroBadgeColor;
            existing.ButtonColor = settings.ButtonColor;
            existing.SurfaceColor = settings.SurfaceColor;
            existing.SurfaceAltColor = settings.SurfaceAltColor;
            existing.InputBgColor = settings.InputBgColor;
            existing.BorderColor = settings.BorderColor;

            existing.DarkPrimaryColor = settings.DarkPrimaryColor;
            existing.DarkSecondaryColor = settings.DarkSecondaryColor;
            existing.DarkAccentColor = settings.DarkAccentColor;
            existing.DarkTextColor = settings.DarkTextColor;
            existing.DarkBgColor = settings.DarkBgColor;
            existing.DarkHeroBadgeColor = settings.DarkHeroBadgeColor;
            existing.DarkButtonColor = settings.DarkButtonColor;
            existing.DarkSurfaceColor = settings.DarkSurfaceColor;
            existing.DarkSurfaceAltColor = settings.DarkSurfaceAltColor;
            existing.DarkInputBgColor = settings.DarkInputBgColor;
            existing.DarkBorderColor = settings.DarkBorderColor;
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
            existing.PrimaryColor = DefaultSettings.PrimaryColor;
            existing.SecondaryColor = DefaultSettings.SecondaryColor;
            existing.AccentColor = DefaultSettings.AccentColor;
            existing.TextColor = DefaultSettings.TextColor;
            existing.LightBgColor = DefaultSettings.LightBgColor;
            existing.HeroBadgeColor = DefaultSettings.HeroBadgeColor;
            existing.ButtonColor = DefaultSettings.ButtonColor;
            existing.SurfaceColor = DefaultSettings.SurfaceColor;
            existing.SurfaceAltColor = DefaultSettings.SurfaceAltColor;
            existing.InputBgColor = DefaultSettings.InputBgColor;
            existing.BorderColor = DefaultSettings.BorderColor;

            existing.DarkPrimaryColor = DefaultSettings.DarkPrimaryColor;
            existing.DarkSecondaryColor = DefaultSettings.DarkSecondaryColor;
            existing.DarkAccentColor = DefaultSettings.DarkAccentColor;
            existing.DarkTextColor = DefaultSettings.DarkTextColor;
            existing.DarkBgColor = DefaultSettings.DarkBgColor;
            existing.DarkHeroBadgeColor = DefaultSettings.DarkHeroBadgeColor;
            existing.DarkButtonColor = DefaultSettings.DarkButtonColor;
            existing.DarkSurfaceColor = DefaultSettings.DarkSurfaceColor;
            existing.DarkSurfaceAltColor = DefaultSettings.DarkSurfaceAltColor;
            existing.DarkInputBgColor = DefaultSettings.DarkInputBgColor;
            existing.DarkBorderColor = DefaultSettings.DarkBorderColor;
            existing.UpdatedAt = DateTime.UtcNow;
            
            context.ColorSettings.Update(existing);
            await context.SaveChangesAsync();
        }
    }

    private static string CoalesceColor(string value, string fallback, ref bool changed)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        changed = true;
        return fallback;
    }
}
