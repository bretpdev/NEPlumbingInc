public interface ICustomAuthenticationService
{
    Task<LoginViewModel?> LoginAsync(string username, string password);
    Task LogoutAsync();
    Task<LoginViewModel?> GetCurrentUserAsync();
}