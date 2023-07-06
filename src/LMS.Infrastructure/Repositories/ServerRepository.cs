using BigBlueApi.Domain.IRepository;
using LIMS.Domain.Entity;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence.Repositories;

public class ServerRepository : IServerRepository
{
    private readonly DbSet<Server> _servers;

    public ServerRepository(LimsContext context) => _servers = context.Set<Server>();

    public async ValueTask<bool> CanJoinServer(long id)
    {
        var server = await _servers.FirstOrDefaultAsync(server => server.Id == id);
        long usersCount = server!.Sessions.Sum(session => session.Users.Count);
        if (server.ServerLimit <= usersCount)
            return false;
        return true;
    }

    public async ValueTask<Server> CreateServer(Server server)
    {
        var newServer = await _servers.AddAsync(server);
        return newServer.Entity;
    }

    public async ValueTask<long> DeleteServer(long Id)
    {
        var server = await _servers.FirstOrDefaultAsync(ser => ser.Id == Id);
        _servers.Remove(server!);
        return server!.Id;
    }

    public async Task EditServer(long id, Server server)
    {
        var newServer = await _servers.FirstOrDefaultAsync(server => server.Id == id);
        newServer!.UpdateServer(server.ServerUrl, server.SharedSecret, server.ServerLimit);
        _servers.Update(newServer!);
    }

    public async ValueTask<Server> GetServer(long Id)
    {
        var Server = await _servers.FirstOrDefaultAsync(ser => ser.Id == Id);
        return Server!;
    }

    public async ValueTask<List<Server>> GetAllServer()
    {
        return await _servers.ToListAsync();
    }

    public async ValueTask<Server> MostCapableServer()
    {
        return await Task.Run(
            () =>
                _servers
                    .OrderBy(
                        server =>
                            server.ServerLimit - server.Sessions.Sum(session => session.Users.Count)
                    )
                    .FirstOrDefault()!
        );
    }
}
