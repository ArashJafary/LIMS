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

    public async Task<long> DeleteServerAsync(long Id)
    {
        var server = await _servers.FirstOrDefaultAsync(ser => ser.Id == Id);
        _servers.Remove(server!);
        return server!.Id;
    }

    public async ValueTask<Server> GetServerAsync(long Id)
    {
        var Server = await _servers.FirstOrDefaultAsync(ser => ser.Id == Id);
        return Server!;
    }

    public async ValueTask<List<Server>> GetAllServersAsync()
        => await _servers.ToListAsync();
}
