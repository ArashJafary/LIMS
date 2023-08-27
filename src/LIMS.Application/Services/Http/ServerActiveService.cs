using LIMS.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using LIMS.Application.Models;
using LIMS.Domain.Entities;
using Microsoft.Extensions.Logging;
using LIMS.Application.Services.Database;

namespace LIMS.Application.Services.Http
{
    public class ServerActiveService
    {
        private readonly ILogger<ServerServiceImpl> _logger;

        public ServerActiveService(ILogger<ServerServiceImpl> logger)
            => _logger = logger;

        public async Task<OperationResult<List<Server>>> SetActiveServers(List<Server> servers) => await Task.Run(() =>
        {
            try
            {
                var downServers = servers.Where(server => !server.IsActive).ToList();

                var ping = new Ping();

                foreach (var server in servers)
                {
                    var reply = ping.Send(server.ServerUrl, 60 * 1000);

                    if (reply.Status != IPStatus.Success)
                        server.SetDownServer();
                }

                return OperationResult<List<Server>>.OnSuccess(servers);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<List<Server>>.OnException(exception);
            }
        });

        public async Task<OperationResult<bool>> IsServerDown(string serverUrl) => await Task.Run(() =>
        {
            try
            {
                var ping = new Ping();

                var reply = ping.Send(serverUrl, 60 * 1000);

                if (reply.Status != IPStatus.Success)
                    return OperationResult<bool>.OnSuccess(true);

                return OperationResult<bool>.OnSuccess(false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<bool>.OnException(exception);
            }
        });
    }
}
