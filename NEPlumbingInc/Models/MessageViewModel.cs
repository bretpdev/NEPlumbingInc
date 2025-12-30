namespace NEPlumbingInc.Models;

public class MessageViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }

    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string Message { get; set; } = string.Empty;

    public string? ResumeBlobName { get; set; }
    public string? ResumeFileName { get; set; }
    public string? ResumeContentType { get; set; }
    public long? ResumeSizeBytes { get; set; }

    public DateTime CreatedAt { get; set; }
    public bool IsSpecialOffer { get; set; }
    public bool IsRead { get; set; }
}