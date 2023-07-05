using BigBlueApi.Domain.IRepositories;
using BigBlueApi.Domain.IRepository;
using LIMS.Domain.Entity;
using LIMS.Domain.IRepositories;

namespace LIMS.Application.Services.Database.BBB
{
    public class MemberShipServiceImp
    {
        private readonly IUnitOfWork _uow;
        private readonly IMemberShipRepository _memberShipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMeetingRepository _meetingRepository;

        public MemberShipServiceImp(IUnitOfWork uow,
            IMemberShipRepository repository,
            IUserRepository userRepository,
            IMeetingRepository meetingRepository)
        {
            _memberShipRepository = repository;
            _uow = uow;
            _userRepository = userRepository;
            _meetingRepository = meetingRepository;
        }

        public async ValueTask<bool> CanJoinUserOnMeetingAsync(long id) =>
            await _memberShipRepository.CanJoinUserOnMeetingAsync(id);

        public async ValueTask<int> JoinUserAsync(string meetingId, int UserId)
        {
            var user = await _userRepository.GetUser(UserId);
            var meeting = await _meetingRepository.FindByMeetingIdAsync(meetingId);
            if (meeting is null)
                return 0;
            var Result = await _memberShipRepository.JoinUserAsync(meeting, user);
            await _uow.SaveChangesAsync();
            return Result;
        }
    }
}
