using LIMS.Domain.Entity;

namespace BigBlueApi.Domain.IRepository;

public interface IServerRepository
{
    ValueTask<bool> CanJoinServer(int id);
    ValueTask<int> DeleteServer(int Id);
    ValueTask<Server> GetServer(int Id);
    ValueTask<List<Server>> GetAllServer();
    ValueTask<Server> CreateServer(Server server);
    ValueTask<Server> MostCapableServer();

    Task EditServer(int id, Server server);
}