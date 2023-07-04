using BigBlueApi.Domain.IRepository;
using BigBlueApi.Persistence.Repository;
using LIMS.Domain.Entity;

namespace BigBlueApi.Application.Services
{
    public class MemberShipServiceImp
    {
        private readonly IUnitOfWork _uow;
        private readonly IMemberShipRepository _memberShipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMeetingRepository _sessionRepository;

        public MemberShipServiceImp(IUnitOfWork uow ,
            IMemberShipRepository repository,
            IUserRepository userRepository,
            IMeetingRepository sessionRepository)
        {
            _memberShipRepository = repository;
            _uow = uow;
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        public async ValueTask<bool> CanJoinUserOnSession(string meetingId) =>
            await _memberShipRepository.CanJoinUserOnSession(meetingId);

        public async ValueTask<int> JoinUser(string MeetingId, int UserId)
        {
            User user = await _userRepository.Find(UserId);
            Meeting session = await _sessionRepository.Find(MeetingId);
            if (session is null)
                return 0;
            var Result = await _memberShipRepository.JoinUser(session, user);
            await _uow.SaveChangesAsync();
            return Result;
        }
    }
}
