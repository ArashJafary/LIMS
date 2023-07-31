using LIMS.Application.DTOs;
using LIMS.Domain.IRepositories;
using LIMS.Application.DTOs;
using LIMS.Domain.Entities;
using LIMS.Application.Models;
using System.Net.NetworkInformation;
using LIMS.Application.Mappers;
using LIMS.Application.Services.Http.BBB;
using System;
using Hangfire.Annotations;
using LIMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace LIMS.Application.Services.Database.BBB;

public class BbbServerServiceImpl
{
    private readonly IServerRepository _servers;
    private readonly ILogger<BbbServerServiceImpl> _logger;
    private readonly BbbServerActiveService _activeService;
    private readonly IUnitOfWork _unitOfWork;

    public BbbServerServiceImpl(
        BbbServerActiveService activeService,
        IServerRepository servers,
        IUnitOfWork unitOfWork,
        ILogger<BbbServerServiceImpl> logger) => (_servers, _activeService, _unitOfWork, _logger) = (servers, activeService, unitOfWork, logger);

    public async ValueTask<OperationResult<bool>> CanJoinServer(long id)
    {
        try
        {
            var server = await _servers
                .GetByIdAsync(id);
            if (server is null)
                return OperationResult<bool>.OnFailed("Server Not Found.");

            if (server.ServerLimit <= server.Meetings.Sum(meeting => meeting.Users.Count))
            {
                _logger.LogWarning($"{server.ServerUrl} is Full Capacity and Cannot Join On That Anymore.");

                return OperationResult<bool>.OnFailed("Cant Join On This Server. It is Full!");
            }

            return OperationResult<bool>.OnSuccess(true);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult<bool>.OnException(exception);
        }
    }

    public async Task<OperationResult> UpdateServer(long id, ServerAddEditDto server)
    {
        try
        {
            if (server is null)
                return OperationResult<Server>.OnFailed("Server Cannot Be Null.");

            var newServer = await _servers.GetByIdAsync(id);

            if (newServer is null)
                return OperationResult<Server>.OnFailed("Server Not Found");

            await newServer.UpdateServer(server.ServerUrl, server.ServerSecret, server.ServerLimit);

            await _unitOfWork.SaveChangesAsync();

            return new OperationResult();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult<Server>.OnException(exception);
        }
    }
    public async ValueTask<OperationResult<long>> CreateServer(ServerAddEditDto serverAddEditDto)
    {
        try
        {
            if (serverAddEditDto is null)
                return OperationResult<long>.OnFailed("Please Send Valid Server Entity.");

            var serverId = await _servers.CreateAsync(ServerDtoMapper.Map(serverAddEditDto));

            await _unitOfWork.SaveChangesAsync();

            return OperationResult<long>.OnSuccess(serverId);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult<long>.OnException(exception);
        }
    }

    public async ValueTask<OperationResult<ServerAddEditDto>> MostCapableServer()
    {
        try
        {
            var capableServer = await _servers.GetCapableAsync();

            var serverDto = ServerDtoMapper.Map(capableServer);

            return OperationResult<ServerAddEditDto>.OnSuccess(serverDto);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult<ServerAddEditDto>.OnException(exception);
        }
    }
    public async ValueTask<OperationResult> DeleteServer(long Id)
    {
        try
        {
            await _servers.DeleteAsync(Id);

            return new OperationResult();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult.OnException(exception);
        }
    }
    public async ValueTask<OperationResult<ServerAddEditDto>> GetServer(long Id)
    {
        try
        {
            var server = ServerDtoMapper.Map(await _servers.GetByIdAsync(Id));

            return OperationResult<ServerAddEditDto>.OnSuccess(server);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult<ServerAddEditDto>.OnException(exception);
        }
    }

    public async ValueTask<OperationResult<List<ServerAddEditDto>>> GetAllServers()
    {
        try
        {
            var servers = await _servers.GetAllAsync();

            var serversDto = new List<ServerAddEditDto>();

            foreach (var server in servers)
                serversDto.Add(ServerDtoMapper.Map(server));

            return OperationResult<List<ServerAddEditDto>>.OnSuccess(serversDto);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult<List<ServerAddEditDto>>.OnException(exception);
        }
    }

    public async Task<OperationResult> UpdateServersForActivate()
    {
        try
        {
            var servers = await _servers.GetAllAsync();

            var checkActiveServers = await _activeService.SetServersActiveIfNotDown(servers);

            if (!checkActiveServers.Success)
                return OperationResult.OnFailed(checkActiveServers.OnFailedMessage);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"All Servers Which Being Down are Checked And Activated.");

            return new OperationResult();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult.OnFailed(exception.Message);
        }
    }

    public async Task<OperationResult<bool>> UpdateServerCheckForBeingDown(string url)
    {
        try
        {
            var server = await _servers.GetByUrlAsync(url);

            var checkServerIsDown = await _activeService.CheckServerBeingDown(server.ServerUrl);

            if (!checkServerIsDown.Result)
            {
                _logger.LogInformation($"{server.ServerUrl} Is Active And Ready For Joining On That.");

                return OperationResult<bool>.OnSuccess(false);
            }

            server.SetDownServer();

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"{server.ServerUrl} Is Downed and Cannot Create Meeting Anymore.");

            return OperationResult<bool>.OnSuccess(true);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult<bool>.OnFailed(exception.Message);
        }
    }
}