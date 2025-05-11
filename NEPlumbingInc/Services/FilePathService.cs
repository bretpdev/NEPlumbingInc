namespace NEPlumbingInc.Services;

public interface IFilePathService
{
    string WwwRootPath { get; }
    string UploadsPath { get; }
    string GetFullPath(string relativePath);
}

public class FilePathService : IFilePathService
{
    public string WwwRootPath { get; }
    public string UploadsPath { get; }

    public FilePathService(IWebHostEnvironment env)
    {
        var solutionDir = env.ContentRootPath
            ?? throw new InvalidOperationException("Could not determine solution directory");
            
        WwwRootPath = Path.Combine(solutionDir, "NEPlumbingInc", "wwwroot");
        UploadsPath = Path.Combine(WwwRootPath, "uploads");
        
        // Ensure directories exist
        Directory.CreateDirectory(UploadsPath);
    }

    public string GetFullPath(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath)) return string.Empty;
        return Path.Combine(WwwRootPath, relativePath.TrimStart('/'));
    }
}