using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using LIMS.Application.Models;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Models;
using LIMS.Application.Services.Record;
using Microsoft.Extensions.Logging;

namespace LIMS.Application.Services.Schedulers.HangFire
{

    public class ServerSchedulerService
    {
        private readonly BbbServerServiceImpl _servers;
        private readonly ILogger<BbbHandleRecordService> _logger;


        public ServerSchedulerService(BbbServerServiceImpl servers, ILogger<BbbHandleRecordService> logger)
            => (_logger, _servers) = (logger, servers);

        public async Task<SingleResponse<string>> SetServerWhichPassive() => await Task.Run(() =>
        {
            try
            {
                RecurringJob.AddOrUpdate(()
                            => _servers.UpdateServersForActivate(), Cron.DayInterval(1));

                _logger.LogInformation("All Server Actives Are Updated For Use.");

                return SingleResponse<string>.OnSuccess("Server is Active.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                return SingleResponse<string>.OnFailed(exception.Message);
            }
        });
    }
}
