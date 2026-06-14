namespace MailSender.Services;

public interface IMailService
{
    Task SendAsync(string to, string subject, string body);
}