using Hangfire;
using LIMS.Application.Models;
using LIMS.Application.Services.Database;
using Microsoft.Extensions.Logging;

namespace LIMS.Application.Services.Schedulers
{

    public class HangfireSchedulerService
    {
        private readonly ServerServiceImpl _servers;
        private readonly ILogger<HangfireSchedulerService> _logger;

        public HangfireSchedulerService(ServerServiceImpl servers, ILogger<HangfireSchedulerService> logger)
            => (_logger, _servers) = (logger, servers);

        public async Task<ResultSingleResponse<string>> SetServerWhichPassive() => await Task.Run(() =>
        {
            try
            {
                RecurringJob.AddOrUpdate(()
                            => _servers.UpdateServersForActivate(), Cron.DayInterval(1));

                _logger.LogInformation("All Server Actives Are Updated For Use.");

                return ResultSingleResponse<string>.OnSuccess("Server is Active.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                return ResultSingleResponse<string>.OnFailed(exception.Message);
            }
        });
    }
}
