using LIMS.Domain.IRepositories;
using LIMS.Application.Models;
using Microsoft.Extensions.Logging;
using LIMS.Domain.Entities;

namespace LIMS.Application.Services.Database
{
    public class RecordServiceImpl
    {
        private readonly IRecordRepository _records;
        private readonly ILogger<RecordServiceImpl> _logger;

        public RecordServiceImpl(IRecordRepository records, ILogger<RecordServiceImpl> logger)
            => (_records, _logger) = (records, logger);

        public async ValueTask<OperationResult<IEnumerable<Record>>> GetAllRecordedVideos()
        {
            var records = await _records.GetAllAsync();

            return OperationResult<IEnumerable<Record>>.OnSuccess(records);
        }

        public async ValueTask<OperationResult<Record>> GetOneRecordedVideo(long id)
        {
            var record = await _records.GetAsync(id);

            return OperationResult<Record>.OnSuccess(record);
        }
    }
}
