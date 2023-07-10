using LIMS.Domain.Entities;

namespace LIMS.Domain.Entity;

public sealed class Server
{
    public int Id { get; set; }
    public string ServerUrl { get; private set; }
    public string SharedSecret { get; private set; }
    public int ServerLimit { get; private set; }
    public bool IsActive { get; private set; }
    public List<Meeting> Meetings { get; set; }

    public Server(string serverUrl, string sharedSecret, int serverLimit)
    {
        IsValid(serverUrl, serverLimit);
        if (serverLimit <= 0)
            throw new ArgumentOutOfRangeException("Server Limit Cant Be Less Than ZERO.");
        ServerUrl = serverUrl;
        SharedSecret = sharedSecret;
        ServerLimit = serverLimit;
    }

    public Server(string serverUrl, int serverLimit)
    {
        IsValid(serverUrl, serverLimit);
        ServerUrl = serverUrl;
        ServerLimit = serverLimit;
    }

    public void UpdateServer(string serverUrl, string sharedSecret, int serverLimit)
    {
        IsValid(serverUrl, serverLimit);
        if (serverLimit <= 0)
            throw new ArgumentOutOfRangeException("Server Limit Cant Be Less Than ZERO.");
        ServerUrl = serverUrl;
        SharedSecret = sharedSecret;
        ServerLimit = serverLimit;
    }

    public async Task SetDownServer()
    {
        await Task.Run(() =>
        {
            IsActive = false;
        });
    }
    private void IsValid(string serverUrl, int serverLimit)
    {
        if (string.IsNullOrWhiteSpace(serverUrl))
            throw new ArgumentNullException($"{nameof(serverUrl)} is Null Or Invalid.");
        if (serverLimit <= 0)
            throw new ArgumentOutOfRangeException("Server Limit Cant Be Less Than ZERO.");
    }
}
