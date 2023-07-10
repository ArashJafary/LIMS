using LIMS.Domain.Entities;
using LIMS.Domain.Entity;

namespace LIMS.Domain.IRepositories;

public interface IMemberShipRepository
{
   ValueTask<MemberShip> GetMemberShip(long userId, long meetingId);
   ValueTask<List<MemberShip>> GetMemberShips();
   ValueTask<long> CreateMemeberShipForSession(Meeting meeting, User user);
}

