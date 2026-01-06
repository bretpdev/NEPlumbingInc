namespace NEPlumbingInc.Models;

public class PageVisit
{
    public int Id { get; set; }

    [Required]
    [MaxLength(2048)]
    public string Path { get; set; } = "/";

    [MaxLength(64)]
    public string? VisitorId { get; set; }

    public DateTime VisitedAtUtc { get; set; } = DateTime.UtcNow;
}
