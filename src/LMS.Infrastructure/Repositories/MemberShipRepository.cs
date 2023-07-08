using BigBlueApi.Domain.IRepositories;
using BigBlueApi.Domain.IRepository;
using LIMS.Domain.Entities;
using LIMS.Domain.Entity;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence.Repositories
{
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly DbSet<MemberShip> _memberShips;
        public MemberShipRepository(LimsContext context)
        {
            _memberShips = context.Set<MemberShip>();
        }
        public async ValueTask<bool> CanJoinUserOnMeetingAsync(long id)
        {
            var session = await _memberShips.FirstOrDefaultAsync(session => session.Meeting.Id == id)!;
            var members = _memberShips.Where(member => member.Meeting.Id == id && member.Meeting.IsRunning).ToList();
            return members.Count <= session!.Meeting.LimitCapacity;
        }
        public async ValueTask<long> JoinUserAsync(Meeting meeting, User user)
        {
            var memberShip = new MemberShip(meeting, user);
            var result = await _memberShips.AddAsync(memberShip);
            return result.Entity.Id;
        }
       public async ValueTask<MemberShip> GetMemberShip(long userId, long meetingId)
            => await _memberShips.FirstOrDefaultAsync(
                member => member.User.Id == userId &&
                member.Meeting.Id == meetingId);
    }
}
