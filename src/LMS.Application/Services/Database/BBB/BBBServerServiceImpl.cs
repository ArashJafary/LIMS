using BigBlueApi.Application.DTOs;
using BigBlueApi.Domain.IRepository;
using LIMS.Application.DTOs;
using LIMS.Domain.Entity;
using LIMS.Domain.Models;

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

    public async ValueTask<OperationResult<bool>> CanJoinServer(long id)
    {
        try
        {
           var canJoinOnServer = await _servers.CanJoinServer(id);
            if (!canJoinOnServer)
            {
                return OperationResult<bool>.OnFailed("Server Not Found.");
            }
            return OperationResult<bool>.OnSuccess(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.OnException(ex);
        }
    }

    public async Task UpdateServer(long id, ServerAddEditDto server)
    {
        var newServer =await _servers.GetServer(id);
        newServer.UpdateServer(server.ServerUrl,server.ServerSecret,server.ServerLimit);
        await _unitOfWork.SaveChangesAsync();
    }
    public async ValueTask<OperationResult<Server>> CreateServer(ServerAddEditDto serverAddEditDto)
    {
        try 
        {
         var server= await _servers.
                CreateServer(ServerDtoMapper.Map(serverAddEditDto));
            await _unitOfWork.SaveChangesAsync();
            if (server is null)
            {
                return OperationResult<Server>.OnFailed("server not find");
            }
            return OperationResult<Server>.OnSuccess(server);
        }
        catch (Exception ex)
        {
            return OperationResult<Server>.OnException(ex);
        }
    }

    public async ValueTask<OperationResult<ServerAddEditDto>> MostCapableServer()
    {
        try
        {
            var server = await _servers.MostCapableServer();
            var serverDto = ServerDtoMapper.Map(server);
            return OperationResult<ServerAddEditDto>.OnSuccess(serverDto);
        }
        catch (Exception ex)
        {
            return OperationResult<ServerAddEditDto>.OnException(ex);
        }
    }
    public async ValueTask<OperationResult<long>> DeleteServer(long Id)
    {
        try
        {
            var ServerId = await  _servers.DeleteServer(Id);
            return OperationResult<long>.OnSuccess(ServerId);  
        }
        catch (Exception ex)
        {
            return OperationResult<long>.OnException(ex);
        }
    }
    public async ValueTask<OperationResult<ServerAddEditDto>> GetServer(long Id)
    {
        try
        {
            var server=  ServerDtoMapper.Map(await _servers.GetServer(Id));
            return OperationResult<ServerAddEditDto>.OnSuccess(server);
        }
        catch (Exception ex) 
        {
            return OperationResult<ServerAddEditDto>.OnException(ex);
        }
    }

    public async ValueTask<OperationResult<List<ServerAddEditDto>>> GetServers()
    {
        try
        {
            var servers= await _servers.GetAllServer();
            var serversDto= new List<ServerAddEditDto>();
            for (long i = 0; i < servers.Count; i++)
            {
                serversDto.Add(ServerDtoMapper.Map(servers[(int)i]));
            }
            return OperationResult<List<ServerAddEditDto>>.OnSuccess(serversDto);
        }
        catch(Exception ex)
        {
            return OperationResult<List<ServerAddEditDto>>.OnException(ex);
        }
    }

    public async Task<OperationResult> SetDownServer(long serverId)
    {
        try
        {
            var server = await _servers.GetServer(serverId);
            await server.SetDownServer();
            return new OperationResult();
        }
        catch (Exception exception)
        {
            return OperationResult.OnFailed(exception.Message);
        }
    }
}