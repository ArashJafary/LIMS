using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entities;

namespace LIMS.Domain.IRepositories
{
    public interface IRecordRepository
    {
        ValueTask<IEnumerable<Record>> GetAllAsync();
        Task<Record> GetAsync(long id);
        ValueTask<long> CreateForMeetingAsync(Record record);
    }
}
