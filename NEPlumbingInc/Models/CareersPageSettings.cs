namespace NEPlumbingInc.Models;

public class CareersPageSettings
{
    public int Id { get; set; }

    public bool IsHiringEnabled { get; set; } = true;

    // Stored as newline-delimited bullet items.
    public string LookingForText { get; set; } = string.Join("\n", new[]
    {
        "Residential and/or commercial plumbing experience",
        "Strong communication and customer service",
        "Dependable, punctual, team-oriented",
        "Valid driver’s license"
    });

    // Stored as newline-delimited bullet items.
    public string HelpfulToIncludeText { get; set; } = string.Join("\n", new[]
    {
        "Years of experience",
        "Licenses/certifications (if applicable)",
        "Availability and preferred work area",
        "Best way/time to contact you"
    });

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
