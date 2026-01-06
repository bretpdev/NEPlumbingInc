using System.Net.Mail;

namespace NEPlumbingInc.Services;

public interface IMessageNotificationSettingsService
{
    Task<MessageNotificationSettings> GetSettingsAsync(CancellationToken cancellationToken = default);
    Task UpdateRecipientEmailsAsync(string newlineDelimitedEmails, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetRecipientEmailsAsync(CancellationToken cancellationToken = default);
}

public class MessageNotificationSettingsService(IDbContextFactory<AppDbContext> contextFactory) : IMessageNotificationSettingsService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public async Task<MessageNotificationSettings> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var settings = await context.MessageNotificationSettings.FirstOrDefaultAsync(cancellationToken);
        if (settings is not null)
            return settings;

        settings = new MessageNotificationSettings();
        context.MessageNotificationSettings.Add(settings);
        await context.SaveChangesAsync(cancellationToken);
        return settings;
    }

    public async Task UpdateRecipientEmailsAsync(string newlineDelimitedEmails, CancellationToken cancellationToken = default)
    {
        var normalized = NormalizeEmails(newlineDelimitedEmails);
        var normalizedText = string.Join("\n", normalized);

        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var settings = await context.MessageNotificationSettings.FirstOrDefaultAsync(cancellationToken);
        if (settings is null)
        {
            settings = new MessageNotificationSettings
            {
                RecipientEmails = normalizedText,
                UpdatedAt = DateTime.UtcNow
            };
            context.MessageNotificationSettings.Add(settings);
        }
        else
        {
            settings.RecipientEmails = normalizedText;
            settings.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetRecipientEmailsAsync(CancellationToken cancellationToken = default)
    {
        var settings = await GetSettingsAsync(cancellationToken);
        return NormalizeEmails(settings.RecipientEmails);
    }

    private static IReadOnlyList<string> NormalizeEmails(string? newlineDelimitedEmails)
    {
        if (string.IsNullOrWhiteSpace(newlineDelimitedEmails))
            return Array.Empty<string>();

        var emails = newlineDelimitedEmails
            .Split(['\r', '\n', ',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(e => e.Trim())
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .Select(e => e.ToLowerInvariant())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var email in emails)
        {
            _ = new MailAddress(email);
        }

        return emails;
    }
}
