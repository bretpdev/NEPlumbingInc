namespace NEPlumbingInc.Models;

public class MessageFormModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    [Required(ErrorMessage = "Message is required")]
    [StringLength(1000, ErrorMessage = "Message is too long")]
    public string Message { get; set; } = string.Empty;
}