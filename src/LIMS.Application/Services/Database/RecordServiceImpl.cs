using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entities;
using LIMS.Domain.IRepositories;
using LIMS.Application.Models;
using Microsoft.Extensions.Logging;

namespace LIMS.Application.Services.Database
{
    public class RecordServiceImpl
    {
        private readonly IRecordRepository _records;
        private readonly ILogger<RecordServiceImpl> _logger;

        public RecordServiceImpl(IRecordRepository records, ILogger<RecordServiceImpl> logger)
            => (_records, _logger) = (records, logger);

        public async ValueTask<IEnumerable<Domain.Entities.Record>> GetAllRecordedVideos()
               => await _records.GetAllAsync();

        public async ValueTask<OperationResult<Domain.Entities.Record>> GetOneRecordedVideo(long id)
        {
            try
            {
                return OperationResult<Domain.Entities.Record>.OnSuccess(await _records.GetAsync(id));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<Domain.Entities.Record>.OnException(exception);
            }
        }
    }
}
