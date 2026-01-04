namespace NEPlumbingInc.Services;

public interface IColorSettingsService
{
    Task<ColorSettings> GetColorSettingsAsync();
    Task<ColorSettings> UpdateColorSettingsAsync(ColorSettings settings);
    Task UpdateFontFamilyAsync(string fontFamily);
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
        // Hero badge follows the link/highlight color (PrimaryColor)
        HeroBadgeColor = "#0066CC",
        HeaderFooterBgColor = "#0066CC",
        ButtonColor = "#0066CC",
        SurfaceColor = "#ffffff",
        SurfaceAltColor = "#f8f9fa",
        InputBgColor = "#ffffff",
        BorderColor = "#dee2e6",
        HeaderFooterTextColor = "#ffffff",
        FontFamily = "Inter",

        // Dark
        DarkPrimaryColor = "#569cd6",
        DarkSecondaryColor = "#4f8cc4",
        DarkAccentColor = "#9cdcfe",
        DarkTextColor = "#d4d4d4",
        DarkBgColor = "#1e1e1e",
        // Hero badge follows the link/highlight color (DarkPrimaryColor)
        DarkHeroBadgeColor = "#569cd6",
        DarkHeaderFooterBgColor = "#2d2d30",
        DarkButtonColor = "#569cd6",
        DarkSurfaceColor = "#252526",
        DarkSurfaceAltColor = "#2d2d30",
        DarkInputBgColor = "#3c3c3c",
        DarkBorderColor = "#3e3e42",
        DarkHeaderFooterTextColor = "#ffffff",
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
                HeaderFooterBgColor = DefaultSettings.HeaderFooterBgColor,
                ButtonColor = DefaultSettings.ButtonColor,
                SurfaceColor = DefaultSettings.SurfaceColor,
                SurfaceAltColor = DefaultSettings.SurfaceAltColor,
                InputBgColor = DefaultSettings.InputBgColor,
                BorderColor = DefaultSettings.BorderColor,
                HeaderFooterTextColor = DefaultSettings.HeaderFooterTextColor,
                FontFamily = DefaultSettings.FontFamily,
                DarkPrimaryColor = DefaultSettings.DarkPrimaryColor,
                DarkSecondaryColor = DefaultSettings.DarkSecondaryColor,
                DarkAccentColor = DefaultSettings.DarkAccentColor,
                DarkTextColor = DefaultSettings.DarkTextColor,
                DarkBgColor = DefaultSettings.DarkBgColor,
                DarkHeroBadgeColor = DefaultSettings.DarkHeroBadgeColor,
                DarkHeaderFooterBgColor = DefaultSettings.DarkHeaderFooterBgColor,
                DarkButtonColor = DefaultSettings.DarkButtonColor,
                DarkSurfaceColor = DefaultSettings.DarkSurfaceColor,
                DarkSurfaceAltColor = DefaultSettings.DarkSurfaceAltColor,
                DarkInputBgColor = DefaultSettings.DarkInputBgColor,
                DarkBorderColor = DefaultSettings.DarkBorderColor,
                DarkHeaderFooterTextColor = DefaultSettings.DarkHeaderFooterTextColor,
                UpdatedAt = DateTime.UtcNow
            };

            // Keep badge synced to links/highlights.
            settings.HeroBadgeColor = settings.PrimaryColor;
            settings.DarkHeroBadgeColor = settings.DarkPrimaryColor;
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
            settings.HeaderFooterBgColor = CoalesceColor(settings.HeaderFooterBgColor, DefaultSettings.HeaderFooterBgColor, ref changed);
            // Force hero badge to follow PrimaryColor (links/highlights)
            var desiredHero = settings.PrimaryColor;
            if (!string.Equals(settings.HeroBadgeColor, desiredHero, StringComparison.OrdinalIgnoreCase))
            {
                settings.HeroBadgeColor = desiredHero;
                changed = true;
            }
            settings.ButtonColor = CoalesceColor(settings.ButtonColor, DefaultSettings.ButtonColor, ref changed);
            settings.SurfaceColor = CoalesceColor(settings.SurfaceColor, DefaultSettings.SurfaceColor, ref changed);
            settings.SurfaceAltColor = CoalesceColor(settings.SurfaceAltColor, DefaultSettings.SurfaceAltColor, ref changed);
            settings.InputBgColor = CoalesceColor(settings.InputBgColor, DefaultSettings.InputBgColor, ref changed);
            settings.BorderColor = CoalesceColor(settings.BorderColor, DefaultSettings.BorderColor, ref changed);
            settings.HeaderFooterTextColor = CoalesceColor(settings.HeaderFooterTextColor, DefaultSettings.HeaderFooterTextColor, ref changed);
            settings.FontFamily = CoalesceString(settings.FontFamily, DefaultSettings.FontFamily, ref changed);

            settings.DarkPrimaryColor = CoalesceColor(settings.DarkPrimaryColor, DefaultSettings.DarkPrimaryColor, ref changed);
            settings.DarkSecondaryColor = CoalesceColor(settings.DarkSecondaryColor, DefaultSettings.DarkSecondaryColor, ref changed);
            settings.DarkAccentColor = CoalesceColor(settings.DarkAccentColor, DefaultSettings.DarkAccentColor, ref changed);
            settings.DarkTextColor = CoalesceColor(settings.DarkTextColor, DefaultSettings.DarkTextColor, ref changed);
            settings.DarkBgColor = CoalesceColor(settings.DarkBgColor, DefaultSettings.DarkBgColor, ref changed);
            settings.DarkHeaderFooterBgColor = CoalesceColor(settings.DarkHeaderFooterBgColor, DefaultSettings.DarkHeaderFooterBgColor, ref changed);
            // Force hero badge to follow DarkPrimaryColor (links/highlights)
            var desiredDarkHero = settings.DarkPrimaryColor;
            if (!string.Equals(settings.DarkHeroBadgeColor, desiredDarkHero, StringComparison.OrdinalIgnoreCase))
            {
                settings.DarkHeroBadgeColor = desiredDarkHero;
                changed = true;
            }
            settings.DarkButtonColor = CoalesceColor(settings.DarkButtonColor, DefaultSettings.DarkButtonColor, ref changed);
            settings.DarkSurfaceColor = CoalesceColor(settings.DarkSurfaceColor, DefaultSettings.DarkSurfaceColor, ref changed);
            settings.DarkSurfaceAltColor = CoalesceColor(settings.DarkSurfaceAltColor, DefaultSettings.DarkSurfaceAltColor, ref changed);
            settings.DarkInputBgColor = CoalesceColor(settings.DarkInputBgColor, DefaultSettings.DarkInputBgColor, ref changed);
            settings.DarkBorderColor = CoalesceColor(settings.DarkBorderColor, DefaultSettings.DarkBorderColor, ref changed);
            settings.DarkHeaderFooterTextColor = CoalesceColor(settings.DarkHeaderFooterTextColor, DefaultSettings.DarkHeaderFooterTextColor, ref changed);

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

        // Enforce: hero badge always matches link/highlight color
        settings.HeroBadgeColor = settings.PrimaryColor;
        settings.DarkHeroBadgeColor = settings.DarkPrimaryColor;
        
        if (existing != null)
        {
            existing.PrimaryColor = settings.PrimaryColor;
            existing.SecondaryColor = settings.SecondaryColor;
            existing.AccentColor = settings.AccentColor;
            existing.TextColor = settings.TextColor;
            existing.LightBgColor = settings.LightBgColor;
            existing.HeroBadgeColor = settings.HeroBadgeColor;
            existing.HeaderFooterBgColor = settings.HeaderFooterBgColor;
            existing.ButtonColor = settings.ButtonColor;
            existing.SurfaceColor = settings.SurfaceColor;
            existing.SurfaceAltColor = settings.SurfaceAltColor;
            existing.InputBgColor = settings.InputBgColor;
            existing.BorderColor = settings.BorderColor;
            existing.HeaderFooterTextColor = settings.HeaderFooterTextColor;
            existing.FontFamily = settings.FontFamily;

            existing.DarkPrimaryColor = settings.DarkPrimaryColor;
            existing.DarkSecondaryColor = settings.DarkSecondaryColor;
            existing.DarkAccentColor = settings.DarkAccentColor;
            existing.DarkTextColor = settings.DarkTextColor;
            existing.DarkBgColor = settings.DarkBgColor;
            existing.DarkHeroBadgeColor = settings.DarkHeroBadgeColor;
            existing.DarkHeaderFooterBgColor = settings.DarkHeaderFooterBgColor;
            existing.DarkButtonColor = settings.DarkButtonColor;
            existing.DarkSurfaceColor = settings.DarkSurfaceColor;
            existing.DarkSurfaceAltColor = settings.DarkSurfaceAltColor;
            existing.DarkInputBgColor = settings.DarkInputBgColor;
            existing.DarkBorderColor = settings.DarkBorderColor;
            existing.DarkHeaderFooterTextColor = settings.DarkHeaderFooterTextColor;
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

    public async Task UpdateFontFamilyAsync(string fontFamily)
    {
        var normalized = string.IsNullOrWhiteSpace(fontFamily) ? DefaultSettings.FontFamily : fontFamily.Trim();

        using var context = await _contextFactory.CreateDbContextAsync();
        var existing = await context.ColorSettings.FirstOrDefaultAsync();

        if (existing is null)
        {
            var settings = new ColorSettings
            {
                PrimaryColor = DefaultSettings.PrimaryColor,
                SecondaryColor = DefaultSettings.SecondaryColor,
                AccentColor = DefaultSettings.AccentColor,
                TextColor = DefaultSettings.TextColor,
                LightBgColor = DefaultSettings.LightBgColor,
                HeroBadgeColor = DefaultSettings.HeroBadgeColor,
                HeaderFooterBgColor = DefaultSettings.HeaderFooterBgColor,
                ButtonColor = DefaultSettings.ButtonColor,
                SurfaceColor = DefaultSettings.SurfaceColor,
                SurfaceAltColor = DefaultSettings.SurfaceAltColor,
                InputBgColor = DefaultSettings.InputBgColor,
                BorderColor = DefaultSettings.BorderColor,
                HeaderFooterTextColor = DefaultSettings.HeaderFooterTextColor,
                FontFamily = normalized,
                DarkPrimaryColor = DefaultSettings.DarkPrimaryColor,
                DarkSecondaryColor = DefaultSettings.DarkSecondaryColor,
                DarkAccentColor = DefaultSettings.DarkAccentColor,
                DarkTextColor = DefaultSettings.DarkTextColor,
                DarkBgColor = DefaultSettings.DarkBgColor,
                DarkHeroBadgeColor = DefaultSettings.DarkHeroBadgeColor,
                DarkHeaderFooterBgColor = DefaultSettings.DarkHeaderFooterBgColor,
                DarkButtonColor = DefaultSettings.DarkButtonColor,
                DarkSurfaceColor = DefaultSettings.DarkSurfaceColor,
                DarkSurfaceAltColor = DefaultSettings.DarkSurfaceAltColor,
                DarkInputBgColor = DefaultSettings.DarkInputBgColor,
                DarkBorderColor = DefaultSettings.DarkBorderColor,
                DarkHeaderFooterTextColor = DefaultSettings.DarkHeaderFooterTextColor,
                UpdatedAt = DateTime.UtcNow,
            };

            context.ColorSettings.Add(settings);
            await context.SaveChangesAsync();
            return;
        }

        existing.FontFamily = normalized;
        existing.UpdatedAt = DateTime.UtcNow;
        context.ColorSettings.Update(existing);
        await context.SaveChangesAsync();
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
            existing.HeaderFooterBgColor = DefaultSettings.HeaderFooterBgColor;
            existing.ButtonColor = DefaultSettings.ButtonColor;
            existing.SurfaceColor = DefaultSettings.SurfaceColor;
            existing.SurfaceAltColor = DefaultSettings.SurfaceAltColor;
            existing.InputBgColor = DefaultSettings.InputBgColor;
            existing.BorderColor = DefaultSettings.BorderColor;
            existing.HeaderFooterTextColor = DefaultSettings.HeaderFooterTextColor;
            existing.FontFamily = DefaultSettings.FontFamily;

            existing.DarkPrimaryColor = DefaultSettings.DarkPrimaryColor;
            existing.DarkSecondaryColor = DefaultSettings.DarkSecondaryColor;
            existing.DarkAccentColor = DefaultSettings.DarkAccentColor;
            existing.DarkTextColor = DefaultSettings.DarkTextColor;
            existing.DarkBgColor = DefaultSettings.DarkBgColor;
            existing.DarkHeroBadgeColor = DefaultSettings.DarkHeroBadgeColor;
            existing.DarkHeaderFooterBgColor = DefaultSettings.DarkHeaderFooterBgColor;
            existing.DarkButtonColor = DefaultSettings.DarkButtonColor;
            existing.DarkSurfaceColor = DefaultSettings.DarkSurfaceColor;
            existing.DarkSurfaceAltColor = DefaultSettings.DarkSurfaceAltColor;
            existing.DarkInputBgColor = DefaultSettings.DarkInputBgColor;
            existing.DarkBorderColor = DefaultSettings.DarkBorderColor;
            existing.DarkHeaderFooterTextColor = DefaultSettings.DarkHeaderFooterTextColor;
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

    private static string CoalesceString(string value, string fallback, ref bool changed)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        changed = true;
        return fallback;
    }
}
