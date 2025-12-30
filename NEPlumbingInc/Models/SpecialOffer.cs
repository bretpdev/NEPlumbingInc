namespace NEPlumbingInc.Models;

public class SpecialOffer
{
    public int Id { get; set; }
    public Guid CampaignId { get; set; } = Guid.Empty;
    public string? IpAddress { get; set; }
    public DateTime ClickedAt { get; set; }
    public bool IsRead { get; set; }
    public bool FormSubmitted { get; set; }
    public DateTime? FormSubmittedAt { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Message { get; set; }
    public const int MaxClicks = 30;

    public void UpdateFromForm(MessageFormModel form)
    {
        Name = form.Name;
        Email = form.Email;
        Phone = form.Phone;
        AddressLine1 = form.AddressLine1;
        AddressLine2 = form.AddressLine2;
        City = form.City;
        State = form.State;
        ZipCode = form.ZipCode;
        Message = form.Message;
        FormSubmitted = true;
        FormSubmittedAt = DateTime.UtcNow;
    }
}