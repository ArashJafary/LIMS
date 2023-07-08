using LIMS.Domain.Entities;
using LIMS.Domain.Entity;

namespace BigBlueApi.Domain.IRepositories;

public interface IMemberShipRepository
{
    ValueTask<bool> CanJoinUserOnMeetingAsync(long meetingId);
   ValueTask<MemberShip> GetMemberShip(long userId, long meetingId);
    ValueTask<long> JoinUserAsync(Meeting meeting, User user);
}

