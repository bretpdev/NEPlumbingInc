using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEPlumbingInc.Data;
using NEPlumbingInc.Models;

namespace NEPlumbingInc.Controllers;

public class NeUndergroundController(IDbContextFactory<AppDbContext> dbFactory) : Controller
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory = dbFactory;

    private const int OfferLimit = 25;

    [HttpPost("/ne-underground/submit")]
    public async Task<IActionResult> Submit([FromForm] UndergroundSubmission submission)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/ne-underground?error=1");
            }

            using var db = await _dbFactory.CreateDbContextAsync();

            var currentCount = await db.UndergroundSubmissions.CountAsync();
            if (currentCount >= OfferLimit)
            {
                return Redirect("/ne-underground?limit=1");
            }

            submission.SubmittedAt = DateTime.UtcNow;
            db.UndergroundSubmissions.Add(submission);
            await db.SaveChangesAsync();

            return Redirect("/ne-underground?sent=1");
        }
        catch
        {
            return Redirect("/ne-underground?error=1");
        }
    }
}
