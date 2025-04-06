public interface ICookieStorageService
{
    Task StoreAuthenticatedUser(AdminUser user);
    Task ClearAuthenticatedUser();
    Task<AdminUser?> GetAuthenticatedUser();
}