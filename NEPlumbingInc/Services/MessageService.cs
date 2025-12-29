namespace NEPlumbingInc.Services;

public interface IMessageService
{
    Task<List<MessageViewModel>> GetAllMessagesAsync();
    Task<MessageViewModel> GetMessageByIdAsync(int id);
    Task<MessageViewModel> CreateMessageAsync(MessageFormModel form, bool isSpecialOffer);
    Task MarkAsReadAsync(int id);
    Task DeleteMessageAsync(int id);
}

public class MessageService(IDbContextFactory<AppDbContext> contextFactory) : IMessageService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public async Task<List<MessageViewModel>> GetAllMessagesAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Messages
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
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Messages.FindAsync(id) 
            ?? throw new KeyNotFoundException($"Message {id} not found");
    }

    public async Task<MessageViewModel> CreateMessageAsync(MessageFormModel form, bool isSpecialOffer)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
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

        context.Messages.Add(message);
        await context.SaveChangesAsync();
        return message;
    }

    public async Task MarkAsReadAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var message = await context.Messages.FindAsync(id)
            ?? throw new KeyNotFoundException($"Message {id} not found");
            
        message.IsRead = true;
        await context.SaveChangesAsync();
    }

    public async Task DeleteMessageAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var message = await context.Messages.FindAsync(id)
            ?? throw new KeyNotFoundException($"Message {id} not found");
            
        context.Messages.Remove(message);
        await context.SaveChangesAsync();
    }
}