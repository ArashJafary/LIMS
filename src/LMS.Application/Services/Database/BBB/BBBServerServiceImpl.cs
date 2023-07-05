using BigBlueApi.Application.DTOs;
using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Models;
using LIMS.Domain.Entity;
using System.Net;

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

    public async ValueTask<OperationResult<bool>> CanJoinServer(int id)
    {
        try
        {
           var Server = await _servers.GetServer(id);
            if (Server is null)
            {
                return OperationResult<bool>.OnFailed("server not find");
            }
            return OperationResult<bool>.OnSuccess(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.OnException(ex);
        }
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

    public async ValueTask<OperationResult> EditServer(int id, ServerAddEditDto serverAddEditDto)
    {
        try
        {
            await _servers.EditServer(id, ServerDtoMapper.Map(serverAddEditDto));
            await _unitOfWork.SaveChangesAsync();
            return new OperationResult();
        }
        catch (Exception ex) 
        {
            return OperationResult.OnException(ex);
        }

    }

    public async ValueTask<OperationResult<ServerAddEditDto>> MostCapableServer()
    {
        try
        {
            var server = await _servers.MostCapableServer();
            var ServerDto = ServerDtoMapper.Map(server);
            return OperationResult<ServerAddEditDto>.OnSuccess(ServerDto);
        }
        catch (Exception ex)
        {
            return OperationResult<ServerAddEditDto>.OnException(ex);
        }
    }
    public async ValueTask<OperationResult<int>> DeleteServer(int Id)
    {
        try
        {
            var ServerId = await  _servers.DeleteServer(Id);
            return OperationResult<int>.OnSuccess(ServerId);  
        }
        catch (Exception ex)
        {
            return OperationResult<int>.OnException(ex);
        }
    }
    public async ValueTask<OperationResult<ServerAddEditDto>> GetServer(int Id)
    {
        try
        {
            var Server=  ServerDtoMapper.Map(await _servers.GetServer(Id));
            return OperationResult<ServerAddEditDto>.OnSuccess(Server);
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
            var Servers= await _servers.GetAllServer();
            var ServersDto= new List<ServerAddEditDto>();
            for (int i = 0; i < Servers.Count; i++)
            {
                ServersDto.Add(ServerDtoMapper.Map(Servers[i]));
            }
            return OperationResult<List<ServerAddEditDto>>.OnSuccess(ServersDto);
        }
        catch(Exception ex)
        {
            return OperationResult<List<ServerAddEditDto>>.OnException(ex);
        }
    }
}