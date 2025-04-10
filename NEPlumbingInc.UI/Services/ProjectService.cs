public interface IProjectService
{
    Task<List<Project>> GetProjectsAsync();
    Task<Project?> GetProjectByIdAsync(int id);
    Task<Project> AddProjectAsync(Project project);
    Task<Project> UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(int id);
}

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.ProjectImages)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.ProjectImages)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project> AddProjectAsync(Project project)
    {
        project.CreatedDate = DateTime.Now;
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        var existingProject = await _context.Projects
            .Include(p => p.ProjectImages)
            .FirstOrDefaultAsync(p => p.Id == project.Id);

        if (existingProject == null)
            throw new KeyNotFoundException($"Project with ID {project.Id} not found.");

        // Update main project properties
        existingProject.Title = project.Title;
        existingProject.Description = project.Description;
        existingProject.ImageUrl = project.ImageUrl;

        // Remove existing images
        _context.ProjectImages.RemoveRange(existingProject.ProjectImages);
        
        // Add new images
        existingProject.ProjectImages = project.ProjectImages
            .Select(pi => new ProjectImage
            {
                ImageUrl = pi.ImageUrl,
                Caption = pi.Caption,
                DisplayOrder = pi.DisplayOrder
            })
            .ToList();

        await _context.SaveChangesAsync();
        return existingProject;
    }

    public async Task DeleteProjectAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}