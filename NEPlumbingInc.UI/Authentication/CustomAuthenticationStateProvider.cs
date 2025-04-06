public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ICookieStorageService _cookieStorageService;
    private bool _isFirstRender = true;

    public CustomAuthenticationStateProvider(ICookieStorageService cookieStorageService)
    {
        _cookieStorageService = cookieStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_isFirstRender)
        {
            _isFirstRender = false;
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var user = await _cookieStorageService.GetAuthenticatedUser();

        var identity = user != null
            ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }, "Cookies")
            : new ClaimsIdentity();

        var principal = new ClaimsPrincipal(identity);
        return new AuthenticationState(principal);
    }

    public void MarkUserAsAuthenticated(AdminUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")  // Example: Add role claim
        };
        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        _cookieStorageService.StoreAuthenticatedUser(user);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public void MarkUserAsLoggedOut()
    {
        _cookieStorageService.ClearAuthenticatedUser();
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
}
