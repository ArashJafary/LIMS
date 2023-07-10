using LIMS.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using LIMS.Application.Models;
using LIMS.Domain.Entity;

namespace LIMS.Application.Services.Http.BBB
{
    public class ServerActiveService
    {
        public async Task<OperationResult<bool>> CheckIsActive(List<Server> servers)
        {
            try
            {
                var ping = new Ping();
                foreach (var server in servers)
                {
                    var reply = ping.Send(server.ServerUrl, 60 * 1000);

                    if (reply.Status != IPStatus.Success)
                        await server.SetDownServer();
                }

                return OperationResult<bool>.OnSuccess(true);
            }
            catch (Exception exception)
            {
                return OperationResult<bool>.OnException(exception);
            }
        }
    }
}
