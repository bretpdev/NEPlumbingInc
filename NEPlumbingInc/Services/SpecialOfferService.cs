public interface ISpecialOfferService
{
    Task<bool> IsOfferAvailableAsync();
    Task<bool> RecordClickAsync(string? ipAddress);
    Task<bool> HasClickedBeforeAsync(string? ipAddress);
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
        return await _context.SpecialOffers.AnyAsync(o => o.IpAddress == ipAddress);
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
}