using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEPlumbingInc.Services;

namespace NEPlumbingInc.Controllers;

[Authorize(Roles = "Admin")]
[ApiExplorerSettings(IgnoreApi = true)]
public class AdminResumesController(IMessageService messageService, IResumeStorageService resumeStorageService) : Controller
{
    private readonly IMessageService _messageService = messageService;
    private readonly IResumeStorageService _resumeStorageService = resumeStorageService;

    [HttpGet("/admin/messages/{id:int}/resume")]
    public async Task<IActionResult> Download(int id, CancellationToken cancellationToken)
    {
        var message = await _messageService.GetMessageByIdAsync(id);

        if (string.IsNullOrWhiteSpace(message.ResumeBlobName))
            return NotFound();

        try
        {
            var (content, contentType) = await _resumeStorageService.OpenResumeReadAsync(message.ResumeBlobName, cancellationToken);
            var downloadName = string.IsNullOrWhiteSpace(message.ResumeFileName) ? "resume" : message.ResumeFileName;
            return File(content, contentType, downloadName);
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }
}
