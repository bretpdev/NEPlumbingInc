public class UndergroundSubmission
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string ZipCode { get; set; } = string.Empty;

    public string? Phone { get; set; } = string.Empty;

    public string? Notes { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}