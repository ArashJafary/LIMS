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

namespace LIMS.Application.Services.Http.BBB
{
    public class BBBServerActiveService
    {
        public async Task<OperationResult<List<Server>>> CheckServersIsActive(List<Server> servers)
        {
            try
            {
                var ping = new Ping();
                foreach (var server in servers)
                {
                    var reply = ping.Send(server.ServerUrl, 60 * 1000);

                    if (reply.Status != IPStatus.Success)
                        await server.SetDownServer();
                    else 
                        await server.SetActiveServer();
                }

                return  OperationResult<List<Server>>.OnSuccess(servers);
            }
            catch (Exception exception)
            {
                return OperationResult<List<Server>>.OnException(exception);
            }
        }


        public async Task<OperationResult<Server>> CheckServerIsActive( Server server)
        {
            try
            {
                var ping = new Ping();
                var reply = ping.Send(server.ServerUrl, 60 * 1000);

                if (reply.Status != IPStatus.Success)
                {
                    await server.SetDownServer();
                    return OperationResult<Server>.OnSuccess(server);
                }
                else
                {
                    await server.SetActiveServer();
                    return OperationResult<Server>.OnSuccess(server);
                }
            }
            catch (Exception exception)
            {
                return OperationResult<Server>.OnException(exception);
            }
        }
    }
}
