using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;

namespace BigBlueApi.Persistence.Repository
{
    public class MemberShipRepository : IMemberShipRepository
    {
        public Task<bool> CanJoinUser(Session session)
        {
            throw new NotImplementedException();
        }

        public Task<int> JoinUser(Session session, User user)
        {
            throw new NotImplementedException();
        }
    }
}
