public class ProjectImage
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; }
    public Project Project { get; set; } = null!;
}