public class AdminUser
{
    public int Id { get; set; }
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public bool IsAuthenticated { get; set; } = false;
    public string Role { get; set; } = string.Empty;
}