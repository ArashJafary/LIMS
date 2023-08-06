using LIMS.Domain.Entities;
using LIMS.Domain.Entities;

namespace LIMS.Domain.IRepositories;

public interface IMemberShipRepository
{
    ValueTask<MemberShip> GetAsync(long userId, long meetingId);
    ValueTask<List<MemberShip>> GetAllAsync();
    ValueTask<long> CreateForMeetingAsync(Meeting meeting, User user);
    ValueTask<long> GetAllMemberShipCountAsync(List<MemberShip> memberShips, Meeting meeting);
}

