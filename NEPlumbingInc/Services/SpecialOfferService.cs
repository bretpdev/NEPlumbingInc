namespace NEPlumbingInc.Services;

public interface ISpecialOfferService
{
    Task<bool> IsOfferAvailableAsync();
    Task<bool> RecordClickAsync(string? ipAddress);
    Task<bool> HasClickedBeforeAsync(string? ipAddress);
    Task<bool> HasSubmittedFormAsync(string? ipAddress);
    Task<bool> RecordFormSubmissionAsync(string? ipAddress, MessageFormModel form);
    Task<(bool hasAccess, string message)> CheckOfferAccessAsync(string? ipAddress);
    Task<int> GetOfferCountAsync();
    Task ResetOfferCountAsync();
    Task StartNewCampaignAsync();
}

public class SpecialOfferService(AppDbContext context, IHttpContextAccessor httpContextAccessor, ISpecialOfferSettingsService settingsService) : ISpecialOfferService
{
    private readonly AppDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ISpecialOfferSettingsService _settingsService = settingsService;

    private async Task<Guid> GetCampaignIdAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        return settings.CampaignId;
    }

    public async Task<bool> IsOfferAvailableAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        
        // Check if offers are enabled
        if (!settings.IsEnabled)
            return false;

        var clickCount = await _context.SpecialOffers
            .CountAsync(o => o.CampaignId == settings.CampaignId);
        return clickCount < settings.MaxOffersLimit;
    }

    public async Task<bool> HasClickedBeforeAsync(string? ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress)) return false;

        var campaignId = await GetCampaignIdAsync();
        
        // Check if they've not only clicked but also submitted the form
        var offer = await _context.SpecialOffers
            .FirstOrDefaultAsync(o => o.CampaignId == campaignId && o.IpAddress == ipAddress);
            
        return offer != null;
    }

    public async Task<bool> RecordClickAsync(string? ipAddress)
    {
        // Don't record if IP has already clicked
        if (await HasClickedBeforeAsync(ipAddress))
        {
            return false;
        }

        // Don't record if we've hit the limit
        if (!await IsOfferAvailableAsync())
        {
            return false;
        }

        var campaignId = await GetCampaignIdAsync();

        var click = new SpecialOffer
        {
            CampaignId = campaignId,
            ClickedAt = DateTime.UtcNow,
            IpAddress = ipAddress
        };

        _context.SpecialOffers.Add(click);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasSubmittedFormAsync(string? ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress)) return false;
        var campaignId = await GetCampaignIdAsync();
        return await _context.SpecialOffers
            .AnyAsync(o => o.CampaignId == campaignId && o.IpAddress == ipAddress && o.FormSubmitted);
    }

    public async Task<bool> RecordFormSubmissionAsync(string? ipAddress, MessageFormModel form)
    {
        var campaignId = await GetCampaignIdAsync();
        var offer = await _context.SpecialOffers
            .FirstOrDefaultAsync(o => o.CampaignId == campaignId && o.IpAddress == ipAddress && !o.FormSubmitted);

        if (offer == null) return false;

        offer.UpdateFromForm(form);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool hasAccess, string message)> CheckOfferAccessAsync(string? ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress))
            return (false, "Unable to determine your location");

        var campaignId = await GetCampaignIdAsync();

        var offer = await _context.SpecialOffers
            .FirstOrDefaultAsync(o => o.CampaignId == campaignId && o.IpAddress == ipAddress);

        if (offer == null)
            return (true, "Welcome to our special offer!");
            
        if (offer.FormSubmitted)
            return (false, "You've already claimed this offer. Thank you!");
            
        return (true, "Welcome back! Please complete your form to claim your offer.");
    }

    public async Task<int> GetOfferCountAsync()
    {
        var campaignId = await GetCampaignIdAsync();
        return await _context.SpecialOffers.CountAsync(o => o.CampaignId == campaignId);
    }

    public async Task ResetOfferCountAsync()
    {
        var offers = await _context.SpecialOffers.ToListAsync();
        _context.SpecialOffers.RemoveRange(offers);
        await _context.SaveChangesAsync();
    }

    public async Task StartNewCampaignAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        settings.CampaignId = Guid.NewGuid();
        settings.UpdatedAt = DateTime.UtcNow;
        await _settingsService.UpdateSettingsAsync(settings);
    }
}