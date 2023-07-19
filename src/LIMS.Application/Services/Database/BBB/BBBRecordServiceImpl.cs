using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entities;
using LIMS.Domain.IRepositories;
using LIMS.Application.Models;

namespace LIMS.Application.Services.Database.BBB
{
    public class BbbRecordServiceImpl
    {
        private readonly IRecordRepository _records;

        public BbbRecordServiceImpl(IRecordRepository records)
            => _records = records;

        public async ValueTask<OperationResult<IEnumerable<Domain.Entities.Record>>> GetAllRecordedVideos()
        {
            try
            {
                return OperationResult<IEnumerable<Domain.Entities.Record>>.OnSuccess(await _records.GetAllRecordsAsync());
            }
            catch (Exception exception)
            {
                return OperationResult<IEnumerable<Domain.Entities.Record>>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<Domain.Entities.Record>> GetOneRecordedVideo(long id)
        {
            try
            {
                return OperationResult<Domain.Entities.Record>.OnSuccess(await _records.GetRecordAsync(id));
            }
            catch (Exception exception)
            {
                return OperationResult<Domain.Entities.Record>.OnException(exception);
            }
        }

    }
}
