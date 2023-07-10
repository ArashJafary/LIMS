using LIMS.Domain.Entity;

namespace LIMS.Domain.IRepositories;
public interface IServerRepository
{
    Task<long> DeleteServerAsync(long Id);
    ValueTask<Server> GetServerAsync(long Id);
    ValueTask<List<Server>> GetAllServersAsync();
    ValueTask<Server> CreateServerAsync(Server server);
}