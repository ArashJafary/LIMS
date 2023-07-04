using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entities;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LIMS.Infrastructure.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly DbSet<Record> _records;

        public RecordRepository(LimsContext context) => _records = context.Set<Record>();
        public async ValueTask<IEnumerable<Record>> GetAllRecordsAsync()
            => await _records.ToListAsync();

        public async Task<Record> GetRecordAsync(long id)
            => await _records.FirstOrDefaultAsync(record => record.Id == id);
    }
}
