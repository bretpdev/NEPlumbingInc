var builder = WebApplication.CreateBuilder(args);

// Add SQLite database service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=appdata.db")); // Database file will be named 'appdata.db'

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Configure authentication (using cookie authentication in this case)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin-login";
        options.LogoutPath = "/admin-logout";
        options.AccessDeniedPath = "/access-denied";
        options.Cookie.Name = ".NEPlumbing.Auth";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

// Configure authorization (for example, protecting admin pages)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

// Register services without circular dependencies
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddScoped<IProjectService, ProjectService>();

// Register IAuthenticationService
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICookieStorageService, CookieStorageService>();
builder.Services.AddLogging();

var app = builder.Build();

// Ensure database and apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await SeedData.Initialize(services, context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

if (app.Environment.IsDevelopment())
{
    app.MapGet("/debug/users", async (AppDbContext db) =>
    {
        var users = await db.AdminUsers.ToListAsync();
        return users.Select(u => new { u.Username, u.Password });
    });
}

app.Run();
