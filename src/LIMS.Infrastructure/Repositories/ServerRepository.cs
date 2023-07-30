using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using LIMS.Domain.Exceptions.Database.BBB;

namespace LIMS.Persistence.Repositories;

public class ServerRepository : IServerRepository
{
    private readonly DbSet<Server> _servers;

    public ServerRepository(LimsContext context) => _servers = context.Set<Server>();

    public async ValueTask<long> CreateAsync(Server server)
    {
        var newServer = await _servers.AddAsync(server);

        ThrowExpectedExceptions(server, false, true);

        return newServer.Entity.Id;
    }

    public async Task DeleteAsync(long id)
    {
        var server = await GetByIdAsync(id);

        ThrowExpectedExceptions(server);

        _servers.Remove(server!);
    }

    public async ValueTask<Server> GetByIdAsync(long id)
    {
        var server = await _servers.FirstOrDefaultAsync(ser => ser.Id == id);

        ThrowExpectedExceptions(server);

        return server!;
    }

    public async ValueTask<Server> GetByUrlAsync(string url)
    {
        var serverByUrl = await _servers.FirstOrDefaultAsync(server => server.ServerUrl == url);

        ThrowExpectedExceptions(serverByUrl);

        return serverByUrl!;
    }

    public async ValueTask<List<Server>> GetAllAsync()
        => await _servers.ToListAsync();

    public async ValueTask<Server> GetCapableAsync() => await Task.Run(() =>
    {
        var capableServer = _servers
                .Where(server => server.IsActive)
                .OrderByDescending(
                    server => server.ServerLimit -
                              server.Meetings.Where(meeting => meeting.IsRunning)
                                  .Sum(meeting => meeting.Users.Count))!
                                        .FirstOrDefault()!;

        ThrowExpectedExceptions(capableServer);

        return capableServer;
    });

    private void ThrowExpectedExceptions(Server? server, bool argumentNullThrow = false, bool createdInDatabase = false)
    {
        if (server is null)
            if (argumentNullThrow)
                throw new ArgumentNullException("Server Cannot Be Null.");
            else if (createdInDatabase)
                throw new EntityConnotAddInDatabaseException("Cannot Create Server On Database");
            else
                throw new NotAnyEntityFoundInDatabaseException("Not Any Server Found With Expected Datas");
    }
}
