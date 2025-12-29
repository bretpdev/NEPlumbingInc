using System.Text;
using Microsoft.AspNetCore.Mvc;
using NEPlumbingInc.Models;
using NEPlumbingInc.Services;

namespace NEPlumbingInc.Controllers;

public class CareersController(IMessageService messageService) : Controller
{
    private readonly IMessageService _messageService = messageService;

    [HttpPost("/careers/submit")]
    public async Task<IActionResult> Submit([FromForm] JobApplicationFormModel form)
    {
        try
        {
            var messageText = BuildApplicationMessage(form);

            var messageForm = new MessageFormModel
            {
                Name = form.Name,
                Email = form.Email,
                Phone = form.Phone,
                Message = messageText
            };

            await _messageService.CreateMessageAsync(messageForm, isSpecialOffer: false);
            return Redirect("/careers?sent=1");
        }
        catch
        {
            return Redirect("/careers?error=1");
        }
    }

    private static string BuildApplicationMessage(JobApplicationFormModel form)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Job Application");
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(form.Position))
            sb.AppendLine($"Position interested in: {form.Position}");

        if (!string.IsNullOrWhiteSpace(form.YearsExperience))
            sb.AppendLine($"Years of experience: {form.YearsExperience}");

        sb.AppendLine($"Licensed/Certified: {(form.LicensedOrCertified ? "Yes" : "No")}");

        if (!string.IsNullOrWhiteSpace(form.Availability))
            sb.AppendLine($"Availability: {form.Availability}");

        if (!string.IsNullOrWhiteSpace(form.PreferredContact))
            sb.AppendLine($"Preferred contact method/time: {form.PreferredContact}");

        sb.AppendLine();
        sb.AppendLine("Additional details:");
        sb.AppendLine(string.IsNullOrWhiteSpace(form.AdditionalDetails) ? "(none provided)" : form.AdditionalDetails.Trim());

        return sb.ToString().TrimEnd();
    }
}
