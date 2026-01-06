using System.ComponentModel.DataAnnotations;

namespace NEPlumbingInc.Models;

public class JobApplicationFormModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [RequiredPhoneNumber]
    public string Phone { get; set; } = string.Empty;

    public string? Position { get; set; }

    public string? YearsExperience { get; set; }

    public bool LicensedOrCertified { get; set; }

    public string? Availability { get; set; }

    public string? PreferredContact { get; set; }

    public string? AdditionalDetails { get; set; }
}
