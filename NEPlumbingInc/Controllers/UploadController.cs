namespace NEPlumbingInc.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly ILogger<UploadController> _logger;
    private readonly IFilePathService _filePathService;

    public UploadController(IFilePathService filePathService, ILogger<UploadController> logger)
    {
        _filePathService = filePathService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(_filePathService.UploadsPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var relativePath = $"/uploads/{fileName}";
            return Ok(relativePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing file upload: {Message}", ex.Message);
            throw;
        }
    }
}