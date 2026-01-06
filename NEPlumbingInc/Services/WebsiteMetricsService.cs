namespace NEPlumbingInc.Services;

public record WebsiteMetricsSummary(
    int VisitsToday,
    int VisitsLast7Days,
    int VisitsLast30Days,
    int VisitsAllTime,
    int UniqueVisitorsToday,
    int UniqueVisitorsLast7Days,
    int UniqueVisitorsLast30Days,
    int UniqueVisitorsAllTime,
    IReadOnlyList<PageHitCount> TopPagesLast30Days);

public record PageHitCount(string Path, int Count);

public interface IWebsiteMetricsService
{
    Task RecordVisitAsync(string path, DateTime visitedAtUtc, string? visitorId, CancellationToken cancellationToken = default);
    Task<WebsiteMetricsSummary> GetSummaryAsync(CancellationToken cancellationToken = default);
}

public class WebsiteMetricsService(IDbContextFactory<AppDbContext> contextFactory) : IWebsiteMetricsService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public async Task RecordVisitAsync(string path, DateTime visitedAtUtc, string? visitorId, CancellationToken cancellationToken = default)
    {
        path = NormalizePath(path);

        if (visitorId is { Length: > 64 })
            visitorId = visitorId[..64];

        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        context.PageVisits.Add(new PageVisit
        {
            Path = path,
            VisitorId = string.IsNullOrWhiteSpace(visitorId) ? null : visitorId,
            VisitedAtUtc = visitedAtUtc
        });
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<WebsiteMetricsSummary> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var todayUtc = DateTime.UtcNow.Date;
        var start7Utc = todayUtc.AddDays(-6);
        var start30Utc = todayUtc.AddDays(-29);

        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        // EF Core DbContext is not thread-safe; run queries sequentially.
        var visitsToday = await context.PageVisits.AsNoTracking().CountAsync(v => v.VisitedAtUtc >= todayUtc, cancellationToken);
        var visitsLast7Days = await context.PageVisits.AsNoTracking().CountAsync(v => v.VisitedAtUtc >= start7Utc, cancellationToken);
        var visitsLast30Days = await context.PageVisits.AsNoTracking().CountAsync(v => v.VisitedAtUtc >= start30Utc, cancellationToken);
        var visitsAllTime = await context.PageVisits.AsNoTracking().CountAsync(cancellationToken);

        var uniqueVisitorsToday = await context.PageVisits.AsNoTracking()
            .Where(v => v.VisitedAtUtc >= todayUtc && v.VisitorId != null)
            .Select(v => v.VisitorId!)
            .Distinct()
            .CountAsync(cancellationToken);

        var uniqueVisitorsLast7Days = await context.PageVisits.AsNoTracking()
            .Where(v => v.VisitedAtUtc >= start7Utc && v.VisitorId != null)
            .Select(v => v.VisitorId!)
            .Distinct()
            .CountAsync(cancellationToken);

        var uniqueVisitorsLast30Days = await context.PageVisits.AsNoTracking()
            .Where(v => v.VisitedAtUtc >= start30Utc && v.VisitorId != null)
            .Select(v => v.VisitorId!)
            .Distinct()
            .CountAsync(cancellationToken);

        var uniqueVisitorsAllTime = await context.PageVisits.AsNoTracking()
            .Where(v => v.VisitorId != null)
            .Select(v => v.VisitorId!)
            .Distinct()
            .CountAsync(cancellationToken);

        var topPagesLast30DaysRaw = await context.PageVisits.AsNoTracking()
            .Where(v => v.VisitedAtUtc >= start30Utc)
            .GroupBy(v => v.Path)
            .Select(g => new { Path = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Path)
            .Take(10)
            .ToListAsync(cancellationToken);

        var topPagesLast30Days = topPagesLast30DaysRaw
            .Select(x => new PageHitCount(x.Path, x.Count))
            .ToList();

        return new WebsiteMetricsSummary(
            visitsToday,
            visitsLast7Days,
            visitsLast30Days,
            visitsAllTime,
            uniqueVisitorsToday,
            uniqueVisitorsLast7Days,
            uniqueVisitorsLast30Days,
            uniqueVisitorsAllTime,
            topPagesLast30Days);
    }

    private static string NormalizePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return "/";

        // Ignore any query/fragment just in case a caller passes a full URL.
        var queryIndex = path.IndexOf('?');
        if (queryIndex >= 0)
            path = path[..queryIndex];

        var fragmentIndex = path.IndexOf('#');
        if (fragmentIndex >= 0)
            path = path[..fragmentIndex];

        if (!path.StartsWith('/'))
            path = "/" + path;

        return path;
    }
}
