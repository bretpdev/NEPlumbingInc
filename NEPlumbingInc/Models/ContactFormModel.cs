public class ContactFormModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "Message is too long.")]
    public string Message { get; set; } = string.Empty;
}