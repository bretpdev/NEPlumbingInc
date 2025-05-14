namespace NEPlumbingInc.Services;

public interface IMessageService
{
    Task<List<MessageViewModel>> GetAllMessagesAsync();
    Task<MessageViewModel> GetMessageByIdAsync(int id);
    Task<MessageViewModel> CreateMessageAsync(MessageFormModel form, bool isSpecialOffer);
    Task MarkAsReadAsync(int id);
    Task DeleteMessageAsync(int id);
}

public class MessageService(AppDbContext context) : IMessageService
{
    private readonly AppDbContext _context = context;

    public async Task<List<MessageViewModel>> GetAllMessagesAsync()
    {
        return await _context.Messages
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MessageViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Message = m.Message,
                CreatedAt = m.CreatedAt,
                IsSpecialOffer = m.IsSpecialOffer,
                IsRead = m.IsRead
            })
            .ToListAsync();
    }

    public async Task<MessageViewModel> GetMessageByIdAsync(int id)
    {
        return await _context.Messages.FindAsync(id) 
            ?? throw new KeyNotFoundException($"Message {id} not found");
    }

    public async Task<MessageViewModel> CreateMessageAsync(MessageFormModel form, bool isSpecialOffer)
    {
        var message = new MessageViewModel
        {
            Name = form.Name,
            Email = form.Email,
            Phone = form.Phone,
            Message = form.Message,
            CreatedAt = DateTime.UtcNow,
            IsSpecialOffer = isSpecialOffer,
            IsRead = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task MarkAsReadAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id)
            ?? throw new KeyNotFoundException($"Message {id} not found");
            
        message.IsRead = true;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMessageAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id)
            ?? throw new KeyNotFoundException($"Message {id} not found");
            
        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
    }
}