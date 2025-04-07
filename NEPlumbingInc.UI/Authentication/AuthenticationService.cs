public class AuthenticationService : IAuthenticationService
{
    private readonly ICookieStorageService _cookieStorageService;
    private readonly CustomAuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationService(ICookieStorageService cookieStorageService, CustomAuthenticationStateProvider authenticationStateProvider)
    {
        _cookieStorageService = cookieStorageService;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<AdminUser?> LoginAsync(string username, string password)
    {
        // Replace this with actual authentication logic (e.g., calling an API)
        // For now, we're assuming "admin" has a role of "Admin" and any other user has a default role.
        var user = new AdminUser
        {
            Username = username,
            Role = username.ToLower() == "admin" ? "Admin" : "User"  // Example of role assignment
        };

        // Store the user in cookies
        await _cookieStorageService.StoreAuthenticatedUser(user);

        // Mark the user as authenticated
        await _authenticationStateProvider.MarkUserAsAuthenticated(user);

        return user;  // Return the authenticated user (or null if authentication fails)
    }

    public async Task LogoutAsync()
    {
        // Clear the user's authentication state and remove the cookie
        await _cookieStorageService.ClearAuthenticatedUser();
        await _authenticationStateProvider.MarkUserAsLoggedOut();
    }
}
