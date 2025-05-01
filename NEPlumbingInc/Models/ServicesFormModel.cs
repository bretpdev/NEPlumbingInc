public class ServicesFormModel
{
    [Key]
    public int Id { get; set; }
    public string? ServiceName { get; set; } = string.Empty;
    public string? ServiceDescription { get; set; } = string.Empty;
    public string? ServiceImage { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<SubServiceModel> SubServices { get; set; } = [];
}

public class SubServiceModel
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public string? Description { get; set; }
    public int ServicesFormModelId { get; set; }
    public ServicesFormModel? ServicesFormModel { get; set; }
}