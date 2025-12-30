using Microsoft.AspNetCore.Mvc;
using NEPlumbingInc.Models;
using NEPlumbingInc.Services;

namespace NEPlumbingInc.Controllers;

public class ContactController(
    IMessageService messageService,
    ISpecialOfferService specialOfferService,
    IHttpContextAccessor httpContextAccessor) : Controller
{
    private readonly IMessageService _messageService = messageService;
    private readonly ISpecialOfferService _specialOfferService = specialOfferService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    [HttpPost("/messages/submit")]
    public async Task<IActionResult> Submit(
        [FromForm] MessageFormModel form,
        [FromForm] string? source,
        [FromForm] string? ip)
    {
        try
        {
            var isSpecialOffer = string.Equals(source, "special-offer", StringComparison.OrdinalIgnoreCase);

            await _messageService.CreateMessageAsync(form, isSpecialOffer);

            if (isSpecialOffer)
            {
                var submissionIp = ip
                    ?? _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

                if (!string.IsNullOrWhiteSpace(submissionIp))
                {
                    await _specialOfferService.RecordFormSubmissionAsync(submissionIp, form);
                }
            }

            return Redirect("/messages?sent=1");
        }
        catch
        {
            return Redirect("/messages?error=1");
        }
    }
}
