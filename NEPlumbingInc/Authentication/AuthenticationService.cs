public class AuthenticationService(
    AppDbContext dbContext,
    CustomAuthenticationStateProvider authenticationStateProvider,
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuthenticationService> logger) : ICustomAuthenticationService
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly CustomAuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ILogger<AuthenticationService> _logger = logger;

    public async Task<LoginViewModel?> LoginAsync(string username, string password)
    {
        try
        {
            _logger.LogInformation("Login attempt for user: {Username}", username);

            var user = await _dbContext.AdminUsers
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user is not null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProps = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext is not null)
                {
                    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);
                    _logger.LogInformation("Login successful for user: {Username}", username);
                    return new LoginViewModel { UserName = user.Username };
                }

                _logger.LogError("HttpContext is null during login for user: {Username}", username);
                return null;
            }

            _logger.LogWarning("Invalid password for user: {Username}", username);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Username}", username);
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is not null)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        _authenticationStateProvider.MarkUserAsLoggedOut();
    }


    public Task<LoginViewModel?> GetCurrentUserAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            var username = user.Identity.Name ?? "Unknown";
            return Task.FromResult<LoginViewModel?>(new LoginViewModel { UserName = username });
        }

        return Task.FromResult<LoginViewModel?>(null);
    }
}