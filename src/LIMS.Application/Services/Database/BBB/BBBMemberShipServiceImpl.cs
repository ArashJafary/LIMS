using LIMS.Application.Mappers;
using LIMS.Domain.IRepositories;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entity;
using LIMS.Domain.IRepositories;
using LIMS.Application.Models;

namespace LIMS.Application.Services.Database.BBB
{
    public class BBBMemberShipServiceImpl
    {
        private readonly IUnitOfWork _uow;
        private readonly IMemberShipRepository _memberShips;
        private readonly IUserRepository _users;
        private readonly IMeetingRepository _meetings;

        public BBBMemberShipServiceImpl(IUnitOfWork uow,
            IMemberShipRepository memberShips,
            IUserRepository users,
            IMeetingRepository meetings)
                => (_uow, _memberShips, _users, _meetings) = (uow, memberShips, users, meetings);

        public async ValueTask<OperationResult<bool>> CanJoinUserOnMeetingAsync(long id)
        {
            try
            {
                var meeting = _meetings.FindAsync(id);
                //var userCountOfMeeting = await meeting.Result.Users
                return OperationResult<bool>.OnSuccess(true);
            }
            catch (Exception exception)
            {
                return OperationResult<bool>.OnException(exception);
            }
        }

        //public async ValueTask<OperationResult<long>> JoinUserAsync(long userId, string meetingId)
        //{
        //    try
        //    {
        //        var user = await _users.GetUser(userId);
        //        if (user is null)
        //            return OperationResult<long>.OnFailed("User is Not Valid.");
        //        var meeting = await _meetings.FindByMeetingIdAsync(meetingId);
        //        if (meeting is null)
        //            return OperationResult<long>.OnFailed("Meeting is Not Valid.");

        //        var result = await _memberShips.JoinUserAsync(meeting, user);
        //        await _uow.SaveChangesAsync();

        //        return OperationResult<long>.OnSuccess(result);
        //    }
        //    catch (Exception exception)
        //    {
        //        return OperationResult<long>.OnException(exception);
        //    }
        //}

        public async ValueTask<OperationResult> BanUserFromMeeting(long userId, string meetingId)
        {
            try
            {
                var user = await _users.GetUser(userId);
                if (user is null)
                    return OperationResult.OnFailed("User is Not Valid.");
                var meeting = await _meetings.FindByMeetingIdAsync(meetingId);
                if (meeting is null)
                    return OperationResult.OnFailed("Meeting is Not Valid.");

                var member =  await _memberShips.GetMemberShip(user.Id, meeting.Id);
                await member.BanUser();
                await _uow.SaveChangesAsync();

                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult.OnException(exception);
            }
        }
    }
}
