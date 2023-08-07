using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LIMS.Domain.Exceptions.Database.BBB;

namespace LIMS.Persistence.Repositories;

public class ServerRepository : IServerRepository
{
    private readonly DbSet<Server> _servers;
    private readonly IMeetingRepository _meetings;
    private readonly IMemberShipRepository _memberShips;

    public ServerRepository(
        IUnitOfWork context,
        IMeetingRepository meetings,
        IMemberShipRepository memberShips) => (_servers, _meetings, _memberShips) = (context.Set<Server>(), meetings, memberShips);

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

    public async ValueTask<List<Server>> GetAllActiveAsync()
       => await _servers.Where(server => server.IsActive).ToListAsync();

    public async ValueTask<List<Server>> GetAllOrderedDescendingAsync(List<Server> activedServers) => await Task.Run(() =>
         activedServers.OrderByDescending(server => server.ServerLimit - _meetings
                  .GetAllRunningsAsync(server)
                  .GetAwaiter()
                  .GetResult()
                      .Sum(meeting => _memberShips
                          .GetAllMemberShipCountAsync(meeting.MemberShips, meeting)
                          .GetAwaiter()
                          .GetResult())).ToList());

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

    public async ValueTask<Server> GetFirstAsync(List<Server> servers) => servers.FirstOrDefault();
}
