namespace BigBlueApi.Domain.IRepository;

public interface IServerRepository
{
    ValueTask<bool> CanJoinServer(int id);
    ValueTask<int> CreateServer(Server server);
    ValueTask EditServer(int id, Server server);
    ValueTask<Server> MostCapableServer();
}
