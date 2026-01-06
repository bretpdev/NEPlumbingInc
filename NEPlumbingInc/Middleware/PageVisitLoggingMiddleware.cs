using Microsoft.AspNetCore.Http;

namespace NEPlumbingInc.Middleware;

public sealed class PageVisitLoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    private const string VisitorCookieName = "neplumbing_vid";

    public async Task InvokeAsync(HttpContext context, IWebsiteMetricsService metrics, ILogger<PageVisitLoggingMiddleware> logger)
    {
        var visitedAtUtc = DateTime.UtcNow;
        var requestPath = context.Request.Path.Value ?? "/";

        string? visitorId = null;
        if (ShouldConsiderVisitorCookie(context, requestPath))
        {
            visitorId = GetOrCreateVisitorId(context);
        }

        await _next(context);

        if (!ShouldLog(context, requestPath))
            return;

        try
        {
            await metrics.RecordVisitAsync(requestPath, visitedAtUtc, visitorId, context.RequestAborted);
        }
        catch (Exception ex)
        {
            // Metrics should never break the site.
            logger.LogDebug(ex, "Failed to record page visit for {Path}", requestPath);
        }
    }

    private static bool ShouldConsiderVisitorCookie(HttpContext context, string requestPath)
    {
        if (!HttpMethods.IsGet(context.Request.Method))
            return false;

        if (IsIgnoredPath(requestPath))
            return false;

        if (HasFileExtension(requestPath))
            return false;

        return true;
    }

    private static string GetOrCreateVisitorId(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue(VisitorCookieName, out var existing) &&
            !string.IsNullOrWhiteSpace(existing) && existing.Length <= 64)
        {
            return existing;
        }

        var newId = Guid.NewGuid().ToString("N");
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = context.Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(10)
        };

        context.Response.Cookies.Append(VisitorCookieName, newId, cookieOptions);
        return newId;
    }

    private static bool ShouldLog(HttpContext context, string requestPath)
    {
        if (!HttpMethods.IsGet(context.Request.Method))
            return false;

        // Only log successful page loads.
        if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
            return false;

        // Filter out known non-page endpoints.
        if (IsIgnoredPath(requestPath))
            return false;

        // Avoid logging static assets by extension.
        if (HasFileExtension(requestPath))
            return false;

        var contentType = context.Response.ContentType;
        if (string.IsNullOrWhiteSpace(contentType))
        {
            // Fallback: most browsers send Accept text/html for page loads.
            var accept = context.Request.Headers.Accept.ToString();
            return accept.Contains("text/html", StringComparison.OrdinalIgnoreCase);
        }

        return contentType.StartsWith("text/html", StringComparison.OrdinalIgnoreCase);
    }

    private static bool HasFileExtension(string path)
    {
        var lastSlash = path.LastIndexOf('/');
        var lastDot = path.LastIndexOf('.');
        return lastDot > lastSlash;
    }

    private static bool IsIgnoredPath(string path)
    {
        // Admin/auth/internal endpoints.
        if (path.StartsWith("/admin", StringComparison.OrdinalIgnoreCase)) return true;
        if (path.StartsWith("/auth", StringComparison.OrdinalIgnoreCase)) return true;
        if (path.StartsWith("/account", StringComparison.OrdinalIgnoreCase)) return true;
        if (path.StartsWith("/_blazor", StringComparison.OrdinalIgnoreCase)) return true;
        if (path.StartsWith("/_framework", StringComparison.OrdinalIgnoreCase)) return true;

        return false;
    }
}
