namespace NEPlumbingInc.Services;

public interface IEmailService
{
    Task SendMessageEmailAsync(MessageFormModel model);
}

public class EmailService(IOptions<EmailSettings> options) : IEmailService
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendMessageEmailAsync(MessageFormModel model)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(_settings.From, _settings.AppPassword),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_settings.From),
            Subject = $"New contact from {model.Name}",
            Body = $"""
                Name: {model.Name}
                Email: {model.Email}
                Phone: {model.Phone}

                Message:
                {model.Message}
            """,
            IsBodyHtml = false
        };

        mail.To.Add(_settings.To);
        mail.ReplyToList.Add(new MailAddress(model.Email));

        await smtpClient.SendMailAsync(mail);
    }
}