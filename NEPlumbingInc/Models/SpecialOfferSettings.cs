namespace NEPlumbingInc.Models;

public class SpecialOfferSettings
{
    [Key]
    public int Id { get; set; }

    [Required]
    public bool IsEnabled { get; set; } = true;

    [Required]
    public string HeadingText { get; set; } = "Welcome to the NE Underground";

    [Required]
    public string DescriptionText { get; set; } = "Unlock this season's exclusive homeowner offer.";

    [Required]
    public string ButtonText { get; set; } = "Reveal the Offer";

    [Required]
    public string NoOfferHeading { get; set; } = "Sorry, you missed it!";

    [Required]
    public string NoOfferDescription { get; set; } = "Our exclusive offer has reached its limit.";

    [Required]
    public string NewsletterText { get; set; } = "Sign up for our newsletter to be notified of future offers.";

    [Required]
    public int MaxOffersLimit { get; set; } = 30;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
