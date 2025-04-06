public interface IAuthenticationService
{
    Task<AdminUser?> LoginAsync(string username, string password);
    Task LogoutAsync();
}