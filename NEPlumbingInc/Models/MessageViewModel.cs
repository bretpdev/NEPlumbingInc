namespace NEPlumbingInc.Models;

public class MessageViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsSpecialOffer { get; set; }
    public bool IsRead { get; set; }
}