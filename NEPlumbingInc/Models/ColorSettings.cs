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

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
