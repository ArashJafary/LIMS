using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entities;
using LIMS.Domain.Exceptions.Database.BBB;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LIMS.Infrastructure.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly DbSet<Record> _records;

        public RecordRepository(LimsContext context)
            => _records = context.Set<Record>();

        public async ValueTask<long> CreateForMeetingAsync(Record record)
        {
            ThrowExpectedExceptions(record, true);

            record.CreateRecord(record.Name, record.Description, record.Meeting);

            await _records.AddAsync(record);

            ThrowExpectedExceptions(record,false,true);

            return record.Id;
        }

        public async ValueTask<IEnumerable<Record>> GetAllAsync()
            => await _records.ToListAsync();

        public async Task<Record> GetAsync(long id)
        {
            var recordById = await _records.FirstOrDefaultAsync(record => record.Id == id);

            ThrowExpectedExceptions(recordById);

            return recordById;
        }

        private void ThrowExpectedExceptions(Record? record, bool argumentNullThrow = false, bool createdInDatabase = false)
        {
            if (record is null)
                if (argumentNullThrow)
                    throw new ArgumentNullException("Server Cannot Be Null.");
                else if (createdInDatabase)
                    throw new EntityConnotAddInDatabaseException("Cannot Create Server On Database");
                else
                    throw new NotAnyEntityFoundInDatabaseException("Not Any Server Found With Expected Datas");
        }
    }
}
