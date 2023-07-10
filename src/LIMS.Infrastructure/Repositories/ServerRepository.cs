using LIMS.Domain.IRepository;
using LIMS.Domain.Entity;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence.Repositories;

public class ServerRepository : IServerRepository
{
    private readonly DbSet<Server> _servers;

    public ServerRepository(LimsContext context) => _servers = context.Set<Server>();

    public async ValueTask<Server> CreateServer(Server server)
    {
        var newServer = await _servers.AddAsync(server);
        return newServer.Entity;
    }

    public async Task<long> DeleteServer(long Id)
    {
        var server = await _servers.FirstOrDefaultAsync(ser => ser.Id == Id);
        _servers.Remove(server!);
        return server!.Id;
    }

    public async ValueTask<Server> GetServer(long Id)
    {
        var Server = await _servers.FirstOrDefaultAsync(ser => ser.Id == Id);
        return Server!;
    }

    public async ValueTask<List<Server>> GetAllServer()
        => await _servers.ToListAsync();
}
