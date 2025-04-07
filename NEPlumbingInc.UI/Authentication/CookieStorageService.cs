public class CookieStorageService : ICookieStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private bool _isInitialized;
    private AdminUser? _cachedUser;
    private readonly ILogger<CookieStorageService> _logger;

    public CookieStorageService(IJSRuntime jsRuntime, ILogger<CookieStorageService> logger)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
        _isInitialized = false;
    }

    public async Task<AdminUser?> GetAuthenticatedUser()
    {
        try
        {
            if (!_isInitialized)
            {
                // Check if we're running on the server during prerendering
                if (_jsRuntime is IJSInProcessRuntime)
                {
                    var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "user");
                    _cachedUser = string.IsNullOrEmpty(userJson) ? null : 
                        JsonSerializer.Deserialize<AdminUser>(userJson);
                }
                _isInitialized = true;
            }

            return _cachedUser;
        }
        catch (JSException ex)
        {
            _logger.LogWarning(ex, "JS interop not available - likely during prerendering");
            return null;
        }
    }


    public async Task StoreAuthenticatedUser(AdminUser user)
    {
        var userJson = JsonSerializer.Serialize(user);
        await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "user", userJson);
        _cachedUser = user;
        _isInitialized = true;
    }

    public async Task ClearAuthenticatedUser()
    {
        await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "user");
        _cachedUser = null;
        _isInitialized = false;
    }
}