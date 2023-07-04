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
        ValueTask<IEnumerable<Record>> GetAllRecordsAsync();
        Task<Record> GetRecordAsync(long id);
    }
}
