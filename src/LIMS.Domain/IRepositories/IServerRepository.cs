using LIMS.Domain.Entities;

namespace LIMS.Domain.IRepositories;
public interface IServerRepository
{
    Task DeleteAsync(long id);
    ValueTask<Server> GetByIdAsync(long id);
    ValueTask<Server> GetByUrlAsync(string url);
    ValueTask<List<Server>> GetAllAsync();
    ValueTask<long> CreateAsync(Server server);
    ValueTask<Server> GetCapableAsync();
}