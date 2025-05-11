namespace NEPlumbingInc.Models;

public interface IServiceManager
{
    Task<List<ServicesFormModel>> GetAllServicesAsync();
    Task<ServicesFormModel> GetServiceByIdAsync(int id);
    Task<ServicesFormModel> CreateServiceAsync(ServicesFormModel service);
    Task<ServicesFormModel> UpdateServiceAsync(ServicesFormModel service);
    Task DeleteServiceAsync(int id);
}

public class ServiceManager(AppDbContext context, IFilePathService filePathService) : IServiceManager
{
    private readonly AppDbContext context = context;
    private readonly IFilePathService _filePathService = filePathService;

    private void DeleteImageFile(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath)) return;

        var fullPath = _filePathService.GetFullPath(imagePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private string NormalizeImagePath(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath)) return string.Empty;

        // If the path is already normalized (starts with /uploads/), return as is
        if (imagePath.StartsWith("/uploads/"))
        {
            return imagePath;
        }

        // Otherwise, get the filename and create a new normalized path
        var fileName = Path.GetFileName(imagePath);
        return $"/uploads/{fileName}";
    }

    public async Task<List<ServicesFormModel>> GetAllServicesAsync()
    {
        return await context.Services.ToListAsync();
    }

    public async Task<ServicesFormModel> GetServiceByIdAsync(int id)
    {
        var service = await context.Services.FindAsync(id)
            ?? throw new KeyNotFoundException($"Service with ID {id} not found.");
        return service;
    }

    public async Task<ServicesFormModel> CreateServiceAsync(ServicesFormModel service)
    {
        service.ServiceImage = NormalizeImagePath(service.ServiceImage);
        context.Services.Add(service);
        await context.SaveChangesAsync();
        return service;
    }

    public async Task<ServicesFormModel> UpdateServiceAsync(ServicesFormModel service)
    {
        var existingService = await context.Services.FindAsync(service.Id)
            ?? throw new KeyNotFoundException($"Service with ID {service.Id} not found.");

        // Delete old image if it exists and a new one is being set
        if (!string.IsNullOrEmpty(existingService.ServiceImage) && 
            existingService.ServiceImage != service.ServiceImage)
        {
            DeleteImageFile(existingService.ServiceImage);
        }

        // Update fields
        existingService.ServiceName = service.ServiceName;
        existingService.ServiceDescription = service.ServiceDescription;
        existingService.ServiceImage = NormalizeImagePath(service.ServiceImage);
        existingService.IsActive = service.IsActive;
        existingService.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existingService;
    }

    public async Task DeleteServiceAsync(int id)
    {
        var service = await context.Services.FindAsync(id)
            ?? throw new KeyNotFoundException($"Service with ID {id} not found.");

        // Delete associated image file
        DeleteImageFile(service.ServiceImage);

        context.Services.Remove(service);
        await context.SaveChangesAsync();
    }
}