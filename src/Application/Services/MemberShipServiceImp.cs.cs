using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Persistence.Repository;

namespace BigBlueApi.Application.Services
{
    public class MemberShipServiceImp
    {
        private readonly IUnitOfWork _uow;
        private readonly IMemberShipRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;

        public MemberShipServiceImp(IUnitOfWork uow ,
            MemberShipRepository repository,
            UserRepository userRepository,
            SessionRepository sessionRepository)
        {
         _repository = repository;
         _uow = uow;
         _userRepository = userRepository;
         _sessionRepository = sessionRepository;
        }

        public async ValueTask<MemberShip> Find(int MemberShipID)
        {
            var memberShip = await _repository.Find(MemberShipID);
            return memberShip;
        }

        public async ValueTask<int> JoinUser(string MeetingId, int UserId)
        {
            User user = await _userRepository.Find(UserId);
            Session session = await _sessionRepository.Find(MeetingId);
            if(session is null)
                return 0;
            var Result = await _repository.JoinUser(session, user);
            await _uow.SaveChangesAsync();
            return Result;
        }

    }
}
