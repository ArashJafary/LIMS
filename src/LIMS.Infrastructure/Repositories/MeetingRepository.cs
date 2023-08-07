using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using LIMS.Domain.Exceptions.Database.BBB;
using Microsoft.Extensions.Logging;

namespace LIMS.Persistence.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly DbSet<Meeting> _meetings;

        public MeetingRepository(IUnitOfWork context)
            => (_meetings) = (context.Set<Meeting>());

        public async ValueTask<string> CreateAsync(Meeting meeting)
        {
            var newMeeting = await _meetings.AddAsync(meeting);

            ThrowExpectedExceptions(newMeeting.Entity, false, true);

            return newMeeting.Entity.MeetingId;
        }

        public async ValueTask<IEnumerable<Meeting>> GetAllAsync() => await _meetings.ToListAsync();

        public async Task DeleteAsync(Meeting meeting) => await Task.Run(() =>
        {
            ThrowExpectedExceptions(meeting, true);

            _meetings.Remove(meeting);
        });

        public async ValueTask<Meeting> GetAsync(long id)
        {
            var meeting = await _meetings.FirstOrDefaultAsync(meeting => meeting.Id == id);

            ThrowExpectedExceptions(meeting);

            return meeting!;
        }

        public async ValueTask<Meeting> GetByMeetingIdAsync(string meetingId)
        {
            var meeting = await _meetings.FirstOrDefaultAsync(meeting => meeting.MeetingId == meetingId);

            ThrowExpectedExceptions(meeting);

            return meeting!;
        }

        public async ValueTask<List<Meeting>> GetAllRunningsAsync(Server server) => await Task.Run(()
            => _meetings.Where(meeting => meeting.IsRunning).ToListAsync());

        private void ThrowExpectedExceptions(Meeting? meeting, bool argumentNullThrow = false, bool createdInDatabase = false)
        {
            if (meeting == null)
                if (argumentNullThrow)
                    throw new ArgumentNullException("Meeting Cannot Be Null.");
                else if (createdInDatabase)
                    throw new EntityConnotAddInDatabaseException("Cannot Create Meeting On Database");
                else
                    throw new NotAnyEntityFoundInDatabaseException("Not Any Meeting Found With Expected Datas");
        }
    }
}
