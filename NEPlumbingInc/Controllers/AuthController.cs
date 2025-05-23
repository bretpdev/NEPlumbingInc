namespace NEPlumbingInc.Controllers;

[ApiController]
[Route("auth")]
[ApiExplorerSettings(IgnoreApi = true)]
public class AuthController(AppDbContext context) : Controller
{
    private readonly AppDbContext context = context;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password, [FromForm] string returnUrl = "/")
    {
        var user = await context.LoginUsers.FirstOrDefaultAsync(u => u.UserName == username);

        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });

            return Redirect(returnUrl);
        }

        // Invalid login attempt
        return Redirect("/account/login?error=1");
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/account/logged-out");
    }
}