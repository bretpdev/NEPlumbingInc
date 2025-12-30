using Microsoft.AspNetCore.Mvc;
using NEPlumbingInc.Services;

namespace NEPlumbingInc.Controllers;

[ApiController]
[Route("api/special-offer")]
public class SpecialOfferController(
    ISpecialOfferService specialOfferService,
    IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private readonly ISpecialOfferService _specialOfferService = specialOfferService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public sealed record RevealResponse(
        bool OfferAvailable,
        string? Message,
        string MessageType,
        string? IpAddress);

    [HttpGet("reveal")]
    public async Task<ActionResult<RevealResponse>> Reveal()
    {
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        var (hasAccess, accessMessage) = await _specialOfferService.CheckOfferAccessAsync(ipAddress);

        if (!hasAccess)
        {
            return Ok(new RevealResponse(
                OfferAvailable: false,
                Message: accessMessage,
                MessageType: "warning",
                IpAddress: ipAddress));
        }

        var clickRecorded = await _specialOfferService.RecordClickAsync(ipAddress);

        if (!clickRecorded && !await _specialOfferService.HasClickedBeforeAsync(ipAddress))
        {
            return Ok(new RevealResponse(
                OfferAvailable: false,
                Message: "This offer is no longer available.",
                MessageType: "warning",
                IpAddress: ipAddress));
        }

        return Ok(new RevealResponse(
            OfferAvailable: true,
            Message: accessMessage,
            MessageType: "info",
            IpAddress: ipAddress));
    }
}
