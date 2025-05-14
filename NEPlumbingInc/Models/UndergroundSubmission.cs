namespace NEPlumbingInc.Models;

public class UndergroundSubmission
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter your full name")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your phone number")]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your ZIP code")]
    [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Enter a valid ZIP code")]
    public string ZipCode { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public DateTime SubmittedAt { get; set; }
}