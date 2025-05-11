public class SpecialOffer
{
    public int Id { get; set; }
    public DateTime ClickedAt { get; set; }
    public string? IpAddress { get; set; }
    public const int MaxClicks = 30;
}