using LIMS.Application.DTOs;
using LIMS.Domain.IRepositories;
using LIMS.Application.DTOs;
using LIMS.Domain.Entities;
using LIMS.Application.Models;
using System.Net.NetworkInformation;
using LIMS.Application.Mappers;
using LIMS.Application.Services.Http.BBB;
using System;
using LIMS.Domain.Entities;

namespace LIMS.Application.Services.Database.BBB;

public class BBBServerServiceImpl
{
    private readonly IServerRepository _servers;
    private readonly BBBServerActiveService _activeService;
    private readonly IUnitOfWork _unitOfWork;

    public BBBServerServiceImpl(BBBServerActiveService activeService, IServerRepository servers,
        IUnitOfWork unitOfWork) => (_servers, _activeService, _unitOfWork) = (servers, activeService, unitOfWork);

    public async ValueTask<OperationResult<bool>> CanJoinServer(long id)
    {
        try
        {
            var server = await _servers.GetServerAsync(id);
            if (server is null)
                return OperationResult<bool>.OnFailed("Server Not Found.");

            if (server.ServerLimit <= server.Meetings.Sum(meeting => meeting.Users.Count))
                return OperationResult<bool>.OnFailed("Cant Join On This Server. It is Full!");

            return OperationResult<bool>.OnSuccess(true);
        }
        catch (Exception exception)
        {
            return OperationResult<bool>.OnException(exception);
        }
    }

    public async Task<OperationResult> UpdateServer(long id, ServerAddEditDto server)
    {
        try
        {
            if (server is null)
                return OperationResult<Server>.OnFailed("server Cant Be Null.");

            var newServer = await _servers.GetServerAsync(id);
            if (newServer is null)
                return OperationResult<Server>.OnFailed("server not find");

            await newServer.UpdateServer(server.ServerUrl, server.ServerSecret, server.ServerLimit);
            await _unitOfWork.SaveChangesAsync();

            return new OperationResult();
        }
        catch (Exception exception)
        {
            return OperationResult<Server>.OnException(exception);
        }
    }
    public async ValueTask<OperationResult<long>> CreateServer(ServerAddEditDto serverAddEditDto)
    {
        try
        {
            if (serverAddEditDto is null)
                return OperationResult<long>.OnFailed("Please Send Valid Server Entity.");

            var serverId = await _servers.
                   CreateServerAsync(ServerDtoMapper.Map(serverAddEditDto));
            await _unitOfWork.SaveChangesAsync();
           
            return OperationResult<long>.OnSuccess(serverId);
        }
        catch (Exception exception)
        {
            return OperationResult<long>.OnException(exception);
        }
    }

    public async ValueTask<OperationResult<ServerAddEditDto>> MostCapableServer()
    {
        try
        {
            var servers = await _servers.GetAllServersAsync();

            var capableServer = servers
                .Where(server=> server.IsActive)
                .OrderByDescending(
                    server => server.ServerLimit - 
                              server.Meetings.Where(meeting => meeting.IsRunning)
                                  .Sum(meeting => meeting.Users.Count))!
                                        .FirstOrDefault();

            var serverDto = ServerDtoMapper.Map(capableServer);

            return OperationResult<ServerAddEditDto>.OnSuccess(serverDto);
        }
        catch (Exception exception)
        {
            return OperationResult<ServerAddEditDto>.OnException(exception);
        }
    }
    public async ValueTask<OperationResult> DeleteServer(long Id)
    {
        try
        { 
            await _servers.DeleteServerAsync(Id);
            return new OperationResult();
        }
        catch (Exception exception)
        {
            return OperationResult.OnException(exception);
        }
    }
    public async ValueTask<OperationResult<ServerAddEditDto>> GetServer(long Id)
    {
        try
        {
            var server = ServerDtoMapper.Map(await _servers.GetServerAsync(Id));
            return OperationResult<ServerAddEditDto>.OnSuccess(server);
        }
        catch (Exception exception)
        {
            return OperationResult<ServerAddEditDto>.OnException(exception);
        }
    }

    public async ValueTask<OperationResult<List<ServerAddEditDto>>> GetServers()
    {
        try
        {
            var servers = await _servers.GetAllServersAsync();
            var serversDto = new List<ServerAddEditDto>();
            for (long i = 0; i < servers.Count; i++)
            {
                serversDto.Add(ServerDtoMapper.Map(servers[(int)i]));
            }
            return OperationResult<List<ServerAddEditDto>>.OnSuccess(serversDto);
        }
        catch (Exception exception)
        {
            return OperationResult<List<ServerAddEditDto>>.OnException(exception);
        }
    }

    public async Task<OperationResult> CheckServers()
    {
        try
        {
            var servers = await _servers.GetAllServersAsync();

            servers=_activeService.CheckServersIsActive(servers).Result.Result;
            await _unitOfWork.SaveChangesAsync();

            return new OperationResult();
        }
        catch (Exception exception)
        {
            return OperationResult.OnFailed(exception.Message);
        }
    }


    public async Task<OperationResult<bool>> CheckServer(string url)
    {
        try
        {
            var server = await _servers.GetServerAsync(url);

           var result=  await _activeService.CheckServerIsActive(server);
            server = result.Result;

            await _unitOfWork.SaveChangesAsync();

            return  OperationResult<bool>.OnSuccess(server.IsActive);
        }
        catch (Exception exception)
        {
            return OperationResult<bool>.OnFailed(exception.Message);
        }
    }
}