public class SpecialOffer
{
    public int Id { get; set; }
    public string? IpAddress { get; set; }
    public DateTime ClickedAt { get; set; }
    public bool IsRead { get; set; }
    public bool FormSubmitted { get; set; }
    public DateTime? FormSubmittedAt { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Message { get; set; }
    public const int MaxClicks = 30;

    public void UpdateFromForm(ContactFormModel form)
    {
        Name = form.Name;
        Email = form.Email;
        Phone = form.Phone;
        Message = form.Message;
        FormSubmitted = true;
        FormSubmittedAt = DateTime.UtcNow;
    }
}