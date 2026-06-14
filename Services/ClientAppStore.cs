using MailSender.Models;

namespace MailSender.Services;

public class ClientAppStore
{
    private readonly Dictionary<string, ClientApp> _apps = new();

    public void Add(ClientApp app)
    {
        _apps[app.AppId] = app;
    }

    public ClientApp? Get(string appId)
    {
        _apps.TryGetValue(appId, out var app);

        return app;
    }
}