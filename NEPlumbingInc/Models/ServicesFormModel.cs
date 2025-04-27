public class ServicesFormModel
{
    [Key]
    public int Id { get; set; }
    public string? ServiceName { get; set; } = string.Empty;
    public string? ServiceDescription { get; set; } = string.Empty;
    public string? ServiceImage { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}