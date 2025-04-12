namespace NEPlumbingInc.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AdminUser?> LoginAsync(string username, string password)
    {
        var user = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);

        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, username),
                    new(ClaimTypes.Role, "Admin")
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            };

            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            user.IsAuthenticated = true;
            return user;
        }

        return null;
    }

    public async Task LogoutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<AdminUser?> GetCurrentUserAsync()
    {
        var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return null;

        return await _context.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);
    }
}