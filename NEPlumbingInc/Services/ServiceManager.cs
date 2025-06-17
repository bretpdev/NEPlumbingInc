namespace NEPlumbingInc.Services;

public interface IServiceManager
{
    Task<List<ServicesFormModel>> GetAllServicesAsync();
    Task<ServicesFormModel> GetServiceByIdAsync(int id);
    Task<ServicesFormModel> CreateServiceAsync(ServicesFormModel service);
    Task<ServicesFormModel> UpdateServiceAsync(ServicesFormModel service);
    Task DeleteServiceAsync(int id);
}

public class ServiceManager(AppDbContext context) : IServiceManager
{
    private readonly AppDbContext _context = context;

    public async Task<List<ServicesFormModel>> GetAllServicesAsync()
    {
        return await _context.Services
            .Include(s => s.SubServices)
            .Select(s => new ServicesFormModel
            {
                Id = s.Id,
                ServiceName = s.ServiceName,
                ServiceDescription = s.ServiceDescription,
                ServiceImage = s.ServiceImage,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                ConsultationType = s.ConsultationType,
                SubServices = s.SubServices!
                    .Select(sub => new SubServiceModel
                    {
                        Id = sub.Id,
                        Name = sub.Name,
                        Description = sub.Description,
                        Price = sub.Price,
                        ServiceId = sub.ServiceId
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<ServicesFormModel> GetServiceByIdAsync(int id)
    {
        var service = await _context.Services.FindAsync(id)
            ?? throw new KeyNotFoundException($"Service with ID {id} not found.");
        return service;
    }

    public async Task<ServicesFormModel> CreateServiceAsync(ServicesFormModel model)
    {
        var service = new ServicesFormModel
        {
            ServiceName = model.ServiceName,
            ServiceDescription = model.ServiceDescription,
            ServiceImage = model.ServiceImage,
            IsActive = model.IsActive,
            CreatedAt = DateTime.UtcNow,
            SubServices = model.SubServices ?? new List<SubServiceModel>()
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task<ServicesFormModel> UpdateServiceAsync(ServicesFormModel model)
    {
        var service = await _context.Services
            .Include(s => s.SubServices)
            .FirstOrDefaultAsync(s => s.Id == model.Id);

        if (service == null)
            throw new KeyNotFoundException($"Service {model.Id} not found");

        // Update main service properties
        service.ServiceName = model.ServiceName;
        service.ServiceDescription = model.ServiceDescription;
        service.ServiceImage = model.ServiceImage;
        service.IsActive = model.IsActive;
        service.ConsultationType = model.ConsultationType;  // Add this line

        // Remove deleted sub-services
        var existingIds = service.SubServices!.Select(s => s.Id).ToList();
        var updatedIds = model.SubServices?.Select(s => s.Id).ToList() ?? new List<int>();
        var toRemove = service.SubServices!.Where(s => !updatedIds.Contains(s.Id)).ToList();
        
        foreach (var subService in toRemove)
        {
            _context.SubServices.Remove(subService);
        }
    
        // Update existing and add new sub-services
        if (model.SubServices != null)
        {
            foreach (var subService in model.SubServices)
            {
                if (subService.Id == 0)
                {
                    // New sub-service
                    service.SubServices!.Add(new SubServiceModel
                    {
                        Name = subService.Name,
                        Description = subService.Description,
                        Price = subService.Price
                    });
                }
                else
                {
                    // Update existing sub-service
                    var existing = service.SubServices!.FirstOrDefault(s => s.Id == subService.Id);
                    if (existing != null)
                    {
                        existing.Name = subService.Name;
                        existing.Description = subService.Description;
                        existing.Price = subService.Price;
                    }
                }
            }
        }
    
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task DeleteServiceAsync(int id)
    {
        var service = await _context.Services.FindAsync(id)
            ?? throw new KeyNotFoundException($"Service with ID {id} not found.");

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
    }
}