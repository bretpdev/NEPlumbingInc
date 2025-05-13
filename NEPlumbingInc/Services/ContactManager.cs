public interface IContactManager
{
    Task<List<ContactViewModel>> GetAllContactsAsync();
    Task<ContactViewModel> GetContactByIdAsync(int id);
    Task MarkAsReadAsync(int id);
    Task DeleteContactAsync(int id);
}

public class ContactManager(AppDbContext context) : IContactManager
{
    private readonly AppDbContext _context = context;

    public async Task<List<ContactViewModel>> GetAllContactsAsync()
    {
        return await _context.SpecialOffers
            .Where(o => o.FormSubmitted)
            .Select(o => new ContactViewModel
            {
                Id = o.Id,
                Name = o.Name ?? string.Empty,
                Email = o.Email ?? string.Empty,
                Phone = o.Phone,
                Message = o.Message ?? string.Empty,
                CreatedAt = o.FormSubmittedAt ?? o.ClickedAt,
                IsSpecialOffer = true,
                IsRead = o.IsRead
            })
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<ContactViewModel> GetContactByIdAsync(int id)
    {
        var offer = await _context.SpecialOffers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Contact {id} not found");

        return new ContactViewModel
        {
            Id = offer.Id,
            Name = offer.Name ?? string.Empty,
            Email = offer.Email ?? string.Empty,
            Phone = offer.Phone,
            Message = offer.Message ?? string.Empty,
            CreatedAt = offer.FormSubmittedAt ?? offer.ClickedAt,
            IsSpecialOffer = true,
            IsRead = offer.IsRead
        };
    }

    public async Task MarkAsReadAsync(int id)
    {
        var offer = await _context.SpecialOffers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Contact {id} not found");

        offer.IsRead = true;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(int id)
    {
        var offer = await _context.SpecialOffers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Contact {id} not found");

        _context.SpecialOffers.Remove(offer);
        await _context.SaveChangesAsync();
    }
}