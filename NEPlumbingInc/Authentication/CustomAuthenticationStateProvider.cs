public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private AdminUser? _currentUser = null;
    public AdminUser GetCurrentUser() => _currentUser ?? throw new InvalidOperationException("Current user is not set.");

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // You can modify this logic to pull the current user from cookies or session
        // For now, assume that the user is already logged in and stored in _currentUser
        var identity = _currentUser != null 
            ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, _currentUser.Username) }, "Cookies")
            : new ClaimsIdentity();

        var principal = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(principal));
    }

    public void MarkUserAsAuthenticated(AdminUser user)
    {
        _currentUser = user;
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")  // Add role claim
        };
        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public void MarkUserAsLoggedOut()
    {
        _currentUser = null;
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
}