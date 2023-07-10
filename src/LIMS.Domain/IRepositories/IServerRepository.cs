using LIMS.Domain.Entity;

namespace LIMS.Domain.IRepositories;
public interface IServerRepository
{
    Task<long> DeleteServer(long Id);
    ValueTask<Server> GetServer(long Id);
    ValueTask<List<Server>> GetAllServer();
    ValueTask<Server> CreateServer(Server server);
}