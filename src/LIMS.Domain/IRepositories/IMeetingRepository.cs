using LIMS.Domain.Entities;
using LIMS.Domain.Entities;

namespace LIMS.Domain.IRepositories;
public interface IMeetingRepository 
{
    ValueTask<string> CreateAsync(Meeting meeting);
    ValueTask<IEnumerable<Meeting>> GetAllAsync();
    Task DeleteAsync(Meeting meeting);
    ValueTask<Meeting> GetAsync(long id);
    ValueTask<Meeting> GetByMeetingIdAsync(string meetingId);
}