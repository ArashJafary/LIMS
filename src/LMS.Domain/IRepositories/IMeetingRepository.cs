using LIMS.Domain.Entity;

namespace LIMS.Domain.IRepositories;
public interface IMeetingRepository 
{
    ValueTask<string> CreateMeetingAsync(Meeting meeting);
    ValueTask<IEnumerable<Meeting>> GetMeetingsAsync();
    Task UpdateMeetingAsync(long id,Meeting meeting);
    Task DeleteMeetingAsync(long id);
    ValueTask<Meeting> FindAsync(long id);
    ValueTask<Meeting> FindByMeetingIdAsync(string meetingId);
    Task EndMeetingAsync(long id);
}