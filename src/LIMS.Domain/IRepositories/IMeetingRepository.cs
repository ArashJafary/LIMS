using LIMS.Domain.Entities;
using LIMS.Domain.Entities;

namespace LIMS.Domain.IRepositories;
public interface IMeetingRepository 
{
    ValueTask<string> CreateMeetingAsync(Meeting meeting);
    ValueTask<IEnumerable<Meeting>> GetMeetingsAsync();
    Task DeleteMeetingAsync(long id);
    ValueTask<Meeting> FindAsync(long id);
    ValueTask<Meeting> FindByMeetingIdAsync(string meetingId);
}