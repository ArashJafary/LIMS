using LIMS.Domain.IRepositories;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using LIMS.Domain.Exceptions;
using LIMS.Domain.Exceptions.Database.BBB;

namespace LIMS.Persistence.Repositories
{
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly DbSet<MemberShip> _memberShips;
        public MemberShipRepository(IUnitOfWork context)
            => _memberShips = context.Set<MemberShip>();

        public async ValueTask<MemberShip> GetAsync(long userId, long meetingId)
        {
            var memberShip = await _memberShips.FirstOrDefaultAsync(
               member => member.User.Id == userId &&
               member.Meeting.Id == meetingId);

            ThrowException(memberShip);

            return memberShip;
        }

        public async ValueTask<List<MemberShip>> GetAllAsync()
            => await _memberShips.ToListAsync();

        public async ValueTask<long> CreateForMeetingAsync(Meeting meeting, User user)
        {
            var memberShip = await _memberShips.AddAsync(new MemberShip(meeting, user));

            ThrowException(memberShip.Entity, true);

            return memberShip.Entity.Id;
        }

        public async ValueTask<long> GetAllMemberShipCountAsync(List<MemberShip> memberShips, Meeting meeting) => await Task.Run(() =>
            memberShips?.Where(memberShip => memberShip.Meeting == meeting && (!memberShip.UserRejected && !memberShip.UserExited))?.Count() ?? 0);

        private void ThrowException(MemberShip? meeting, bool notCreatedInDatabase = false)
        {
            if (meeting is null)
                if (notCreatedInDatabase)
                    throw new EntityConnotAddInDatabaseException("Cannot Create Meeting On Database");
                else
                    throw new NotAnyEntityFoundInDatabaseException("Not Any Meeting Found With Expected Datas");
        }
    }
}
