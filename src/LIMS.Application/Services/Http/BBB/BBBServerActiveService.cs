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

namespace LIMS.Application.Services.Http.BBB
{
    public class BBBServerActiveService
    {
        public async Task<OperationResult<List<Server>>> SetServersActiveIfAreNotDown(List<Server> servers)
        {
            try
            {
                var downServers = servers.Where(server => !server.IsActive).ToList();

                var ping = new Ping();
                foreach (var server in servers)
                {
                    var reply = ping
                        .Send(server.ServerUrl, 60 * 1000);

                    if (reply.Status != IPStatus.Success)
                        await server.SetDownServer();
                }

                return OperationResult<List<Server>>.OnSuccess(servers);
            }
            catch (Exception exception)
            {
                return OperationResult<List<Server>>.OnException(exception);
            }
        }

        public async Task<OperationResult<bool>> CheckBeingDownServer(string serverUrl)
        {
            try
            {
                var ping = new Ping();
                var reply = ping
                    .Send(serverUrl, 60 * 1000);

                if (reply.Status != IPStatus.Success)
                    return OperationResult<bool>.OnSuccess(true);
                else
                    return OperationResult<bool>.OnFailed("Server is Active.");
            }
            catch (Exception exception)
            {
                return OperationResult<bool>.OnException(exception);
            }
        }
    }
}
