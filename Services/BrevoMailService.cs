using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MailSender.Services;

public class BrevoMailService : IMailService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public BrevoMailService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var apiKey = _configuration["Brevo:ApiKey"];
        var senderEmail = _configuration["Brevo:SenderEmail"];
        var senderName = _configuration["Brevo:SenderName"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Brevo API key is not configured.");
        }

        if (string.IsNullOrWhiteSpace(senderEmail))
        {
            throw new InvalidOperationException("Brevo sender email is not configured.");
        }

        var requestBody = new
        {
            sender = new
            {
                email = senderEmail,
                name = senderName
            },
            to = new[]
            {
                new
                {
                    email = to
                }
            },
            subject = subject,
            htmlContent = body
        };

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.brevo.com/v3/smtp/email"
        );

        request.Headers.Add("api-key", apiKey);
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json")
        );

        request.Content = JsonContent.Create(requestBody);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();

            throw new InvalidOperationException(
                $"Brevo error: {(int)response.StatusCode} {errorContent}"
            );
        }
    }
}