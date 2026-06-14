namespace MailSender.Models;

public class SendMailResponse
{
    public string AppId { get; set; } = string.Empty;

    public string AppName { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public EmailDto Email { get; set; } = new();
}