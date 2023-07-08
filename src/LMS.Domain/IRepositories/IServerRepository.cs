using LIMS.Domain.Entity;

namespace BigBlueApi.Domain.IRepository;

public interface IServerRepository
{
    ValueTask<bool> CanJoinServer(long id);
    Task<long> DeleteServer(long Id);
    ValueTask<Server> GetServer(long Id);
    ValueTask<List<Server>> GetAllServer();
    ValueTask<Server> CreateServer(Server server);
    ValueTask<Server> MostCapableServer();
}