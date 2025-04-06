public class CookieStorageService : ICookieStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private bool _isInitialized;

    public CookieStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _isInitialized = false;
    }

    public async Task<AdminUser?> GetAuthenticatedUser()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            return null; // Prevent early access before OnAfterRenderAsync
        }

        var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "user");
        return string.IsNullOrEmpty(userJson) ? null : JsonSerializer.Deserialize<AdminUser>(userJson);
    }

    public async Task StoreAuthenticatedUser(AdminUser user)
    {
        var userJson = JsonSerializer.Serialize(user);
        await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "user", userJson);
    }

    public async Task ClearAuthenticatedUser()
    {
        await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "user");
    }
}