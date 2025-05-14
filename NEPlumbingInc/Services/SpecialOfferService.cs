namespace NEPlumbingInc.Services;

public interface ISpecialOfferService
{
    Task<bool> IsOfferAvailableAsync();
    Task<bool> RecordClickAsync(string? ipAddress);
    Task<bool> HasClickedBeforeAsync(string? ipAddress);
    Task<bool> HasSubmittedFormAsync(string? ipAddress);
    Task<bool> RecordFormSubmissionAsync(string? ipAddress, MessageFormModel form);
    Task<(bool hasAccess, string message)> CheckOfferAccessAsync(string? ipAddress);
}

public class SpecialOfferService(AppDbContext context, IHttpContextAccessor httpContextAccessor) : ISpecialOfferService
{
    private readonly AppDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<bool> IsOfferAvailableAsync()
    {
        var clickCount = await _context.SpecialOffers.CountAsync();
        return clickCount < SpecialOffer.MaxClicks;
    }

    public async Task<bool> HasClickedBeforeAsync(string? ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress)) return false;
        
        // Check if they've not only clicked but also submitted the form
        var offer = await _context.SpecialOffers
            .FirstOrDefaultAsync(o => o.IpAddress == ipAddress);
            
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

        var click = new SpecialOffer
        {
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
        return await _context.SpecialOffers
            .AnyAsync(o => o.IpAddress == ipAddress && o.FormSubmitted);
    }

    public async Task<bool> RecordFormSubmissionAsync(string? ipAddress, MessageFormModel form)
    {
        var offer = await _context.SpecialOffers
            .FirstOrDefaultAsync(o => o.IpAddress == ipAddress && !o.FormSubmitted);

        if (offer == null) return false;

        offer.UpdateFromForm(form);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool hasAccess, string message)> CheckOfferAccessAsync(string? ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress))
            return (false, "Unable to determine your location");

        var offer = await _context.SpecialOffers
            .FirstOrDefaultAsync(o => o.IpAddress == ipAddress);

        if (offer == null)
            return (true, "Welcome to our special offer!");
            
        if (offer.FormSubmitted)
            return (false, "You've already claimed this offer. Thank you!");
            
        return (true, "Welcome back! Please complete your form to claim your offer.");
    }
}