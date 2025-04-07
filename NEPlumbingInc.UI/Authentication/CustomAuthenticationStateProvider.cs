public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ICookieStorageService _cookieStorageService;
    private readonly ILogger<CustomAuthenticationStateProvider> _logger;
    private AuthenticationState? _cachedAuthState;

    public CustomAuthenticationStateProvider(
        ICookieStorageService cookieStorageService,
        ILogger<CustomAuthenticationStateProvider> logger)
    {
        _cookieStorageService = cookieStorageService;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Return cached state if available
        if (_cachedAuthState != null)
        {
            return _cachedAuthState;
        }

        try
        {
            var user = await _cookieStorageService.GetAuthenticatedUser();
            var state = await CreateAuthenticationState(user);
            _cachedAuthState = state;
            return state;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting authentication state");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    private Task<AuthenticationState> CreateAuthenticationState(AdminUser? user)
    {
        if (user == null)
        {
            _logger.LogInformation("No authenticated user found");
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        _logger.LogInformation("User authenticated: {Username}", user.Username);
        return Task.FromResult(new AuthenticationState(principal));
    }

    public async Task MarkUserAsAuthenticated(AdminUser user)
    {
        var state = await CreateAuthenticationState(user);
        _cachedAuthState = state;
        await _cookieStorageService.StoreAuthenticatedUser(user);
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    public async Task MarkUserAsLoggedOut()
    {
        _cachedAuthState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        await _cookieStorageService.ClearAuthenticatedUser();
        NotifyAuthenticationStateChanged(Task.FromResult(_cachedAuthState));
    }
}