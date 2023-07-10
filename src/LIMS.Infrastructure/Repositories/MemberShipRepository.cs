using LIMS.Domain.IRepositories;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.Entity;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LIMS.Persistence.Repositories
{
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly DbSet<MemberShip> _memberShips;
        public MemberShipRepository(LimsContext context)
            => _memberShips = context.Set<MemberShip>();

        public async ValueTask<MemberShip> GetMemberShipAsync(long userId, long meetingId)
            => await _memberShips.FirstOrDefaultAsync(
                member => member.User.Id == userId &&
                member.Meeting.Id == meetingId);

        public async ValueTask<List<MemberShip>> GetMemberShipsAsync()  
            => await _memberShips.ToListAsync();

        public async ValueTask<long> CreateMemeberShipForMeetingAsync(Meeting meeting, User user)
        {
            var memeberShip = await _memberShips.AddAsync(new MemberShip(meeting, user));
            return memeberShip.Entity.Id;
        }
    }
}
