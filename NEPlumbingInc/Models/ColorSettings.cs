namespace NEPlumbingInc.Models;

public class ColorSettings
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string PrimaryColor { get; set; } = "#0066CC";

    [Required]
    public string SecondaryColor { get; set; } = "#005299";

    [Required]
    public string AccentColor { get; set; } = "#003d7a";

    [Required]
    public string TextColor { get; set; } = "#212529";

    [Required]
    public string LightBgColor { get; set; } = "#f8f9fa";

    [Required]
    public string HeroBadgeColor { get; set; } = "#0056b3";

    [Required]
    public string ButtonColor { get; set; } = "#0066CC";

    [Required]
    public string SurfaceColor { get; set; } = "#ffffff";

    [Required]
    public string SurfaceAltColor { get; set; } = "#f8f9fa";

    [Required]
    public string InputBgColor { get; set; } = "#ffffff";

    [Required]
    public string BorderColor { get; set; } = "#dee2e6";

    [Required]
    public string HeaderFooterTextColor { get; set; } = "#ffffff";

    [Required]
    public string DarkPrimaryColor { get; set; } = "#569cd6";

    [Required]
    public string DarkSecondaryColor { get; set; } = "#4f8cc4";

    [Required]
    public string DarkAccentColor { get; set; } = "#9cdcfe";

    [Required]
    public string DarkTextColor { get; set; } = "#d4d4d4";

    [Required]
    public string DarkBgColor { get; set; } = "#1e1e1e";

    [Required]
    public string DarkHeroBadgeColor { get; set; } = "#2d2d30";

    [Required]
    public string DarkButtonColor { get; set; } = "#569cd6";

    [Required]
    public string DarkSurfaceColor { get; set; } = "#252526";

    [Required]
    public string DarkSurfaceAltColor { get; set; } = "#2d2d30";

    [Required]
    public string DarkInputBgColor { get; set; } = "#3c3c3c";

    [Required]
    public string DarkBorderColor { get; set; } = "#3e3e42";

    [Required]
    public string DarkHeaderFooterTextColor { get; set; } = "#ffffff";

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
