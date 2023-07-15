using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using LIMS.Application.Models;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Models;

namespace LIMS.Application.Services.Schedulers.HangFire
{
    
    public class ServerSchedulerService
    {
        private readonly BBBServerServiceImpl _servers;

        public ServerSchedulerService(BBBServerServiceImpl servers)
            => _servers = servers;
        public async Task<SingleResponse<string>> SetServerIsPassive()
        {
            try
            {
                RecurringJob.AddOrUpdate(() => _servers.SetDownServers(), Cron.DayInterval(1));
                return SingleResponse<string>.OnSuccess("Server is Active.");
            }
            catch (Exception exception)
            {
                return SingleResponse<string>.OnFailed(exception.Message);
            }
        }
    }
}
