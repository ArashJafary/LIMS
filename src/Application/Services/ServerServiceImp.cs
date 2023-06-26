using BigBlueApi.Application.Mappers;
using BigBlueApi.Application.Models;
using BigBlueApi.Domain.IRepository;

namespace BigBlueApi.Application.Services;

public class ServerServiceImp
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServerRepository _servers;

    public ServerServiceImp(IUnitOfWork unitOfWork, IServerRepository servers) =>
        (_unitOfWork, _servers) = (unitOfWork, servers);

    public async ValueTask<int> CreateServer(ServerAddEditDto server)
    {
        await _servers.CreateServer(ServerDtoMapper.Map(server));
        return await _unitOfWork.SaveChangesAsync();
    }

    public async ValueTask EditServer(int id, ServerAddEditDto server)
    {
        await _servers.EditServer(id, ServerDtoMapper.Map(server));
        await _unitOfWork.SaveChangesAsync();
    }

    public async ValueTask<bool> CanUserJoinServer(int id) => await _servers.CanJoinServer(id);

    public async Task<ServerAddEditDto> MostCapableServer() =>
        ServerDtoMapper.Map(await _servers.MostCapableServer());
}
