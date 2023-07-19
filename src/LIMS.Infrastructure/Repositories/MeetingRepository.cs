using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LIMS.Persistence.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly DbSet<Meeting> _meetings;
        public MeetingRepository(LimsContext context) 
            => _meetings = context.Set<Meeting>();

        public async ValueTask<string> CreateMeetingAsync(Meeting meeting)
        {
            var newMeeting = await _meetings.AddAsync(meeting);
            return newMeeting.Entity.MeetingId;
        }

        public async ValueTask<IEnumerable<Meeting>> GetMeetingsAsync() => await _meetings.ToListAsync();

        public async Task DeleteMeetingAsync(long id)
        {
            var meeting = await FindAsync(id);
            _meetings.Remove(meeting);
        }

        public async ValueTask<Meeting> FindAsync(long id) =>
            await _meetings.FirstOrDefaultAsync(meeting => meeting.Id == id);

        public async ValueTask<Meeting> FindByMeetingIdAsync(string meetingId)
           => await _meetings.FirstOrDefaultAsync(meeting => meeting.MeetingId == meetingId);
    }
}
