using BigBlueApi.Domain.IRepository;
using LIMS.Domain.Entity;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly DbSet<Meeting> _meetings;
        public MeetingRepository(LimsContext context) => _meetings = context.Set<Meeting>();

        public async ValueTask<string> CreateMeetingAsync(Meeting meeting)
        {
            var newMeeting = await _meetings.AddAsync(meeting);
            return newMeeting.Entity.MeetingId;
        }

        public async ValueTask<IEnumerable<Meeting>> GetMeetingsAsync() => await _meetings.ToListAsync();

        public async Task UpdateMeetingAsync(long id, Meeting meeting)
        {
            var newMeeting = await _meetings.FirstOrDefaultAsync(meet => meet.Id == id);
            newMeeting.Update(
                meeting.Name,
                meeting.ModeratorPassword,
                meeting.AttendeePassword,
                meeting.EndDateTime,
                meeting.LimitCapacity
            );
            _meetings.Update(newMeeting);
        }

        public async Task DeleteMeetingAsync(long id)
            => _meetings.Remove(await _meetings.FirstOrDefaultAsync(meeting => meeting.Id == id)!);

        public async ValueTask<Meeting> FindAsync(long id) =>
            await _meetings.FirstOrDefaultAsync(meeting => meeting.Id == id);

        public async ValueTask<Meeting> FindByMeetingIdAsync(string meetingId)
           => await _meetings.FirstOrDefaultAsync(meeting => meeting.MeetingId == meetingId);

        public async Task EndMeetingAsync(string meetingId)
        {
            var meeting = await _meetings.FirstOrDefaultAsync(meeting => meeting.MeetingId == meetingId);
            meeting!.EndSession(DateTime.Now);
        }
    }
}
