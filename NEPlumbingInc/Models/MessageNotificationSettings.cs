namespace NEPlumbingInc.Models;

public class MessageNotificationSettings
{
    public int Id { get; set; }

    // Newline-delimited list of recipient email addresses.
    public string RecipientEmails { get; set; } = "";

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
