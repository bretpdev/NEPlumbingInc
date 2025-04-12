public class Services
{
    public int Id { get; set; }
    public required string ServiceName { get; set; }
    public string? ServiceDescription { get; set; }
    public string? ServiceImage { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}