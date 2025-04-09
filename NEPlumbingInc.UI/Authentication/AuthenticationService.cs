public class AuthenticationService : IAuthenticationService
{
    private readonly ICookieStorageService _cookieStorageService;
    private readonly CustomAuthenticationStateProvider _authenticationStateProvider;
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticationService(
        ICookieStorageService cookieStorageService,
        CustomAuthenticationStateProvider authenticationStateProvider,
        AppDbContext context,
        IPasswordHasher passwordHasher)
    {
        _cookieStorageService = cookieStorageService;
        _authenticationStateProvider = authenticationStateProvider;
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<AdminUser?> LoginAsync(string username, string password)
    {
        // Find user in database
        var user = await _context.AdminUsers
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

        // Verify user exists and password matches
        if (user != null && _passwordHasher.VerifyPassword(password, user.PasswordHash))
        {
            user.IsAuthenticated = true;
            await _context.SaveChangesAsync();

            // Store the authenticated user in cookies
            await _cookieStorageService.StoreAuthenticatedUser(user);

            // Mark the user as authenticated
            await _authenticationStateProvider.MarkUserAsAuthenticated(user);

            return user;
        }

        return null;
    }

    public async Task LogoutAsync()
    {
        // Clear the user's authentication state and remove the cookie
        await _cookieStorageService.ClearAuthenticatedUser();
        await _authenticationStateProvider.MarkUserAsLoggedOut();
    }
}
