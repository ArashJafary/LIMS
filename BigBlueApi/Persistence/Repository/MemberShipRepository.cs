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

        public async ValueTask<MemberShip> Find(int MemberShipID)
        {
            var memberShip = await _memberShips.FirstOrDefaultAsync(msh => msh.Id == MemberShipID);
            return memberShip!;
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
