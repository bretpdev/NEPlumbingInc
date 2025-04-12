public class AuthenticationService : IAuthenticationService
{
 private readonly AppDbContext _dbContext;
    private readonly CustomAuthenticationStateProvider _authenticationStateProvider;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        AppDbContext dbContext, 
        CustomAuthenticationStateProvider authenticationStateProvider,
        ILogger<AuthenticationService> logger)
    {
        _dbContext = dbContext;
        _authenticationStateProvider = authenticationStateProvider;
        _logger = logger;
    }

    public async Task<AdminUser?> LoginAsync(string username, string password)
    {
        try
        {
            _logger.LogInformation("Login attempt for user: {Username}", username);

            var user = await _dbContext.AdminUsers
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", username);
                return null;
            }

            // Verify the hashed password
            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                _logger.LogInformation("Login successful for user: {Username}", username);
                _authenticationStateProvider.MarkUserAsAuthenticated(user);
                return user;
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

    public Task LogoutAsync()
    {
        // Mark the user as logged out
        _authenticationStateProvider.MarkUserAsLoggedOut();
        return Task.CompletedTask;
    }

    public Task<AdminUser?> GetCurrentUserAsync()
    {
        // You can retrieve the current user by querying the database using the claims in the identity.
        // But since we are storing the user in the authentication state, we just need to return it here.
        return Task.FromResult((AdminUser?)_authenticationStateProvider.GetCurrentUser());
    }
}