using LIMS.Domain.Entities;

namespace LIMS.Domain.IRepositories;
public interface IServerRepository
{
    Task DeleteServerAsync(long Id);
    ValueTask<Server> GetServerAsync(long Id);
    ValueTask<Server> GetServerWithUrlAsync(string url);
    ValueTask<List<Server>> GetAllServersAsync();
    ValueTask<long> CreateServerAsync(Server server);
}