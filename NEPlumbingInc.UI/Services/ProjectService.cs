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
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await _context.Projects.FindAsync(id);
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
        var existingProject = await _context.Projects.FindAsync(project.Id);
        if (existingProject == null)
            throw new KeyNotFoundException($"Project with ID {project.Id} not found.");

        existingProject.Title = project.Title;
        existingProject.Description = project.Description;
        existingProject.ImageUrl = project.ImageUrl;

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