using BigBlueApi.Application.DTOs;
using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;

namespace LIMS.Application.Services.Database.BBB;

public class BBBServerServiceImpl
{
    private readonly IServerRepository _servers;
    private readonly IUnitOfWork _unitOfWork;

    public BBBServerServiceImpl(IServerRepository serverRepository, IUnitOfWork unitOfWork)
    {
        _servers = serverRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<bool> CanJoinServer(int id)
    {
        return await _servers.CanJoinServer(id);
    }

    public async ValueTask<int> CreateServer(ServerAddEditDto serverAddEditDto)
    {
        var server = await _servers.CreateServer(ServerDtoMapper.Map(serverAddEditDto));
        await _unitOfWork.SaveChangesAsync();
        return server.Id;
    }

    public async ValueTask EditServer(int id, ServerAddEditDto serverAddEditDto)
    {
        await _servers.EditServer(id, ServerDtoMapper.Map(serverAddEditDto));
        await _unitOfWork.SaveChangesAsync();
    }

    public async ValueTask<ServerAddEditDto> MostCapableServer()
    {
        var server = await _servers.MostCapableServer();
        var ServerDto = ServerDtoMapper.Map(server);
        return ServerDto;
    }
}