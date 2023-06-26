using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence.Repository;

public class ServerRepository : IServerRepository
{
    private readonly DbSet<Server> _servers;

    public ServerRepository(BigBlueContext context) => _servers = context.Set<Server>();

    public async ValueTask<bool> CanJoinServer(int id)
    {
        var server = await _servers.FirstOrDefaultAsync(server => server.Id == id);
        int usersCount = server.Sessions.Sum(session => session.Users.Count);
        if (server.ServerLimit <= usersCount)
            return false;
        return true;
    }

    public async ValueTask<int> CreateServer(Server server)
    {
        var newServer = await _servers.AddAsync(server);
        return newServer.Entity.Id;
    }

    public async ValueTask EditServer(int id, Server server)
    {
        var newServer = await _servers.FirstOrDefaultAsync(server => server.Id == id);
        newServer!.UpdateServer(server.ServerUrl,server.SharedSecret, server.ServerLimit);
        _servers.Update(newServer);
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
