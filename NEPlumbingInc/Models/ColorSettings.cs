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

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
