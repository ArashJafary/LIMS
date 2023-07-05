using BigBlueApi.Application.Mappers;
using BigBlueApi.Domain.IRepositories;
using BigBlueApi.Domain.IRepository;
using LIMS.Domain.Entity;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Models;

namespace LIMS.Application.Services.Database.BBB
{
    public class MemberShipServiceImp
    {
        private readonly IUnitOfWork _uow;
        private readonly IMemberShipRepository _memberShips;
        private readonly IUserRepository _users;
        private readonly IMeetingRepository _meetings;

        public MemberShipServiceImp(IUnitOfWork uow,
            IMemberShipRepository memberShips,
            IUserRepository users,
            IMeetingRepository meetings)
                => (_uow, _memberShips, _users, _meetings) = (uow, memberShips, users, meetings);

        public async ValueTask<OperationResult<bool>> CanJoinUserOnMeetingAsync(long id)
        {
            try
            {
                await _memberShips.CanJoinUserOnMeetingAsync(id);
                return OperationResult<bool>.OnSuccess(true);
            }
            catch (Exception exception)
            {
                return OperationResult<bool>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<long>> JoinUserAsync(long userId, string meetingId)
        {
            try
            {
                var user = await _users.GetUser(userId);
                if (user is null)
                    return OperationResult<long>.OnFailed("User is Not Valid.");
                var meeting = await _meetings.FindByMeetingIdAsync(meetingId);
                if (meeting is null)
                    return OperationResult<long>.OnFailed("Meeting is Not Valid.");

                var result = await _memberShips.JoinUserAsync(meeting, user);
                await _uow.SaveChangesAsync();

                return OperationResult<long>.OnSuccess(result);
            }
            catch (Exception exception)
            {
                return OperationResult<long>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult> BanUserFromMeeting(long userId, string meetingId)
        {
            try
            {
                var user = await _users.GetUser(userId);
                if (user is null)
                    return OperationResult<long>.OnFailed("User is Not Valid.");
                var meeting = await _meetings.FindByMeetingIdAsync(meetingId);
                if (meeting is null)
                    return OperationResult<long>.OnFailed("Meeting is Not Valid.");

                await _memberShips.BanUserAsync(user.Id, meeting.Id);
                await _uow.SaveChangesAsync();

                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult<long>.OnException(exception);
            }
        }
    }
}
