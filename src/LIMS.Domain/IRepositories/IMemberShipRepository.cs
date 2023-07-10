using LIMS.Domain.Entities;
using LIMS.Domain.Entity;

namespace LIMS.Domain.IRepositories;

public interface IMemberShipRepository
{
   ValueTask<MemberShip> GetMemberShipAsync(long userId, long meetingId);
   ValueTask<List<MemberShip>> GetMemberShipsAsync();
   ValueTask<long> CreateMemeberShipForMeetingAsync(Meeting meeting, User user);
}

