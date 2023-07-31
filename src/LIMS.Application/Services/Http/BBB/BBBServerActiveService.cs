using LIMS.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using LIMS.Application.Models;
using LIMS.Application.Services.Database.BBB;
using LIMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace LIMS.Application.Services.Http.BBB
{
    public class BbbServerActiveService
    {
        private readonly ILogger<BbbServerServiceImpl> _logger;
        public BbbServerActiveService(ILogger<BbbServerServiceImpl> logger) 
            => _logger = logger;

        public async Task<OperationResult<List<Server>>> SetServersActiveIfNotDown(List<Server> servers)
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
        }

        public async Task<OperationResult<bool>> CheckServerBeingDown(string serverUrl)
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
        }
    }
}
