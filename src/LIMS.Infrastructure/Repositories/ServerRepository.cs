using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LIMS.Persistence.Repositories;

public class ServerRepository : IServerRepository
{
    private readonly DbSet<Server> _servers;

    public ServerRepository(LimsContext context) => _servers = context.Set<Server>();

    public async ValueTask<long> CreateServerAsync(Server server)
    {
        var newServer = await _servers.AddAsync(server);
        return newServer.Entity.Id;
    }

    public async Task DeleteServerAsync(long id)
    {
        var server =await GetServerAsync(id);
        _servers.Remove(server!);
    }

    public async ValueTask<Server> GetServerAsync(long id)
    {
        var Server = await _servers.FirstOrDefaultAsync(ser => ser.Id == id);
        return Server!;
    }

    public async ValueTask<Server> GetServerWithUrlAsync(string url)
    {
        var Server = await _servers.FirstOrDefaultAsync(ser => ser.ServerUrl == url);
        return Server!;
    }

    public async ValueTask<List<Server>> GetAllServersAsync()
        => await _servers.ToListAsync();
}
