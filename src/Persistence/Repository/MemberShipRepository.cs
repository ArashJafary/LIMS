using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence.Repository
{
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly DbSet<MemberShip> _memberShips;
        public MemberShipRepository(BigBlueContext context)
        {
            _memberShips = context.Set<MemberShip>();
        }

        public async ValueTask<bool> CanJoinUserOnSession(string meetingId)
        {
            int limitSession = _memberShips.FirstOrDefault(session => session.Session.MeetingId == meetingId)!.Session.LimitCapacity;
            int membersCount = _memberShips.Where(member => member.Session.MeetingId == meetingId && member.Session.IsRunning).Count();
            return membersCount <= limitSession;
        }

        public async ValueTask<int> JoinUser(Session session, User user)
        {
            var MemberShip= new MemberShip()
            {
                Session=session,
                User=user
            };
            var Result= await _memberShips.AddAsync(MemberShip);
            return Result.Entity.Id;
        }
    }
}
