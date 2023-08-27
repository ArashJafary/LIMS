using LIMS.Domain.Entities;

namespace LIMS.Domain.IRepositories;
public interface IServerRepository
{
    Task DeleteAsync(long id);
    ValueTask<Server> GetByIdAsync(long id);
    ValueTask<Server> GetByUrlAsync(string url);
    ValueTask<List<Server>> GetAllAsync();
    ValueTask<long> CreateAsync(Server server);
    ValueTask<List<Server>> GetAllActiveAsync();
    ValueTask<List<Server>> GetAllOrderedDescendingAsync(List<Server> activedServers);
    ValueTask<Server> GetFirstAsync(List<Server> servers);
    Task<bool> CanJoinOnServerAsync(Server server);
}