using LIMS.Domain.Entity;

namespace BigBlueApi.Domain.IRepository;

public interface IServerRepository
{
    ValueTask<bool> CanJoinServer(int id);
    ValueTask<Server> CreateServer(Server server);
    ValueTask EditServer(int id, Server server);
    ValueTask<Server> MostCapableServer();
}
