namespace BigBlueApi.Domain;

public class Server
{
    public int Id { get; set; }
    public string ServerUrl { get; private set; }
    public string SharedSecret { get; private set; }
    public int ServerLimit { get; private set; }
    public List<Session> Sessions { get; set; }

    public Server(string serverUrl, string sharedSecret, int serverLimit)
    {
        IsValid(serverUrl, sharedSecret, serverLimit);
        ServerUrl = serverUrl;
        SharedSecret = sharedSecret;
        ServerLimit = serverLimit;
    }
    public Server(string serverUrl, int serverLimit)
    {
        IsValid(serverUrl, serverLimit);
        ServerUrl = serverUrl;
        ServerLimit = serverLimit;
        if (string.IsNullOrWhiteSpace(serverUrl))
            throw new ArgumentNullException($"{nameof(serverUrl)} is Null Or Invalid.");
    }

    public void UpdateServer(string serverUrl, string sharedSecret, int serverLimit)
    {
        IsValid(serverUrl,sharedSecret,serverLimit);
        ServerUrl = serverUrl;
        SharedSecret = sharedSecret;
        ServerLimit = serverLimit;
    }
    private void IsValid(string serverUrl, string sharedSecret, int serverLimit)
    {
        if (string.IsNullOrWhiteSpace(serverUrl))
            throw new ArgumentNullException($"{nameof(serverUrl)} is Null Or Invalid.");
        if (string.IsNullOrEmpty(sharedSecret))
            throw new ArgumentNullException($"{nameof(sharedSecret)} is Null Or Invalid");
        if (serverLimit <= 0)
            throw new ArgumentOutOfRangeException("Server Limit Cant Be Less Than ZERO.");
    }
    private void IsValid(string serverUrl, int serverLimit)
    {
        if (string.IsNullOrWhiteSpace(serverUrl))
            throw new ArgumentNullException($"{nameof(serverUrl)} is Null Or Invalid.");
        if (serverLimit <= 0)
            throw new ArgumentOutOfRangeException("Server Limit Cant Be Less Than ZERO.");
    }
}