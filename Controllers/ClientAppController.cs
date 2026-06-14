using MailSender.Models;
using MailSender.Services;
using Microsoft.AspNetCore.Mvc;

namespace MailSender.Controllers;

[ApiController]
[Route("client-app")]
public class ClientAppController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ClientAppStore _clientAppStore;
    private readonly JwtService _jwtService;

    public ClientAppController(
        IConfiguration configuration,
        ClientAppStore clientAppStore,
        JwtService jwtService)
    {
        _configuration = configuration;
        _clientAppStore = clientAppStore;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterClientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult Register(RegisterClientRequest request)
    {
        var validPassword = _configuration["RegistrationPassword"];

        if (request.Pass != validPassword)
        {
            return StatusCode(403, new
            {
                error = "Invalid index-based password 25"
            });
        }

        var app = new ClientApp
        {
            AppId = request.AppId,
            AppName = request.AppName
        };

        _clientAppStore.Add(app);

        var token = _jwtService.GenerateToken(app.AppId, app.AppName);

        var response = new RegisterClientResponse
        {
            AppId = app.AppId,
            AppName = app.AppName,
            Key = token
        };

        return Created("", response);
    }
}