using MailSender.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MailSender.Services;

namespace MailSender.Controllers;

[ApiController]
[Route("mail")]
public class MailController : ControllerBase
{
    private readonly IMailService _mailService;
    private readonly ILogger<MailController> _logger;

    public MailController(IMailService mailService,
    ILogger<MailController> logger)
    {
        _mailService = mailService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("send")]
    public async Task<IActionResult> Send(SendMailRequest request)
    {
        var appId = User.FindFirst("appId")?.Value;
        var appName = User.FindFirst("appName")?.Value;

        if (appId == null || appName == null)
        {
            return Unauthorized();
        }

        var subject = request.Subject;
        var body = request.Body;

        if (subject.EndsWith("?"))
        {
            subject = "[Q] " + subject;
        }

        if (body.Contains("Szynal"))
        {
            body = body.Replace(
                "Szynal",
                "[student.Szynal]Szynal[/student.Szynal]"
            );
        }
        // Funkcja ta wysyła maila przez servis Brevo
        await _mailService.SendAsync(request.To, subject, body);

        // Kolejna ważna linijka (mail dodatko pojawia się w terminalu)
        _logger.LogInformation(
             "Mail sent/queued by app {AppId} - {AppName}. To: {To}, Subject: {Subject}, Body: {Body}",
            appId,
            appName,
            request.To,
            subject,
            body
        );

        var response = new SendMailResponse
        {
            AppId = appId,
            AppName = appName,
            Status = "queued",
            Email = new EmailDto
            {
                To = request.To,
                Subject = subject,
                Body = body
            }
        };

        return Accepted(response);
    }
}