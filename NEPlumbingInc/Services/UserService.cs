namespace NEPlumbingInc.Services;

public interface IUserService
{
    Task<List<UserManagementModel>> GetAllUsersAsync();
    Task<UserManagementModel> GetUserByIdAsync(int id);
    Task<UserManagementModel> CreateUserAsync(string username, string password);
    Task<bool> UpdateUserPasswordAsync(int id, string newPassword);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> UserExists(string username);
}

public class UserService(IDbContextFactory<AppDbContext> contextFactory) : IUserService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public async Task<List<UserManagementModel>> GetAllUsersAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.LoginUsers
            .Select(u => new UserManagementModel
            {
                Id = u.Id,
                UserName = u.UserName,
                CreatedAt = DateTime.Now // LoginViewModel doesn't have CreatedAt, so we'll use Now as fallback
            })
            .OrderBy(u => u.UserName)
            .ToListAsync();
    }

    public async Task<UserManagementModel> GetUserByIdAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var user = await context.LoginUsers.FindAsync(id)
            ?? throw new KeyNotFoundException($"User with ID {id} not found");
        return new UserManagementModel
        {
            Id = user.Id,
            UserName = user.UserName,
            CreatedAt = DateTime.Now
        };
    }

    public async Task<UserManagementModel> CreateUserAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        using var context = await _contextFactory.CreateDbContextAsync();
        
        // Check if username already exists
        var existingUser = await context.LoginUsers.FirstOrDefaultAsync(u => u.UserName == username);
        if (existingUser != null)
            throw new InvalidOperationException($"Username '{username}' already exists");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var newUser = new LoginViewModel
        {
            UserName = username,
            Password = hashedPassword
        };

        context.LoginUsers.Add(newUser);
        await context.SaveChangesAsync();

        return new UserManagementModel
        {
            Id = newUser.Id,
            UserName = newUser.UserName,
            CreatedAt = DateTime.Now
        };
    }

    public async Task<bool> UpdateUserPasswordAsync(int id, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("Password cannot be empty", nameof(newPassword));

        using var context = await _contextFactory.CreateDbContextAsync();
        var user = await context.LoginUsers.FindAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        context.LoginUsers.Update(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var user = await context.LoginUsers.FindAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        // Don't allow deleting the last admin user
        var userCount = await context.LoginUsers.CountAsync();
        if (userCount <= 1)
            throw new InvalidOperationException("Cannot delete the last admin user");

        context.LoginUsers.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UserExists(string username)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.LoginUsers.AnyAsync(u => u.UserName == username);
    }
}

public class UserManagementModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
