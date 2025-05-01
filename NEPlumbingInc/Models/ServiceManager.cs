namespace NEPlumbingInc.Models;

public interface IServiceManager
{
    Task<List<ServicesFormModel>> GetAllServicesAsync();
    Task<ServicesFormModel> GetServiceByIdAsync(int id);
    Task<ServicesFormModel> CreateServiceAsync(ServicesFormModel service);
    Task<ServicesFormModel> UpdateServiceAsync(ServicesFormModel service);
    Task DeleteServiceAsync(int id);
}

public class ServiceManager(AppDbContext appDbContext) : IServiceManager
{
    private readonly AppDbContext context = appDbContext;

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
        context.Services.Add(service);
        await context.SaveChangesAsync();
        return service;
    }

    public async Task<ServicesFormModel> UpdateServiceAsync(ServicesFormModel service)
    {
        var existingService = await context.Services.FindAsync(service.Id)
            ?? throw new KeyNotFoundException($"Service with ID {service.Id} not found.");

        // Update fields
        existingService.ServiceName = service.ServiceName;
        existingService.ServiceDescription = service.ServiceDescription;
        existingService.ServiceImage = service.ServiceImage;
        existingService.IsActive = service.IsActive;
        existingService.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existingService;
    }

    public async Task DeleteServiceAsync(int id)
    {
        var service = await context.Services.FindAsync(id)
            ?? throw new KeyNotFoundException($"Service with ID {id} not found.");

        context.Services.Remove(service);
        await context.SaveChangesAsync();
    }
}