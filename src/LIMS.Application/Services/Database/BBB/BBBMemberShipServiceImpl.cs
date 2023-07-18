using LIMS.Application.Mappers;
using LIMS.Domain.IRepositories;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.IRepositories;
using LIMS.Application.Models;

namespace LIMS.Application.Services.Database.BBB
{
    public class BBBMemberShipServiceImpl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemberShipRepository _memberShips;
        private readonly IUserRepository _users;
        private readonly IMeetingRepository _meetings;

        public BBBMemberShipServiceImpl(
            IUnitOfWork unitOfWork,
            IMemberShipRepository memberShips,
            IUserRepository users,
            IMeetingRepository meetings)
                => (_unitOfWork, _memberShips, _users, _meetings) 
                    = (unitOfWork, memberShips, users, meetings);

        public async ValueTask<OperationResult<bool>> CanJoinUserOnMeeting(long meetingId)
        {
            try
            {
                var meeting = await _meetings.FindAsync(meetingId);
                if (meeting is null)
                    return OperationResult<bool>.OnFailed("Meeting Not Found.");

                var memberShips = await _memberShips.GetMemberShipsAsync();
                if (memberShips is null)
                    return OperationResult<bool>.OnFailed("No Joining Found.");

                var userCountOfMeeting = memberShips.Where(memberShip => memberShip.Meeting.Id == meetingId)!.Count();
                if (meeting.LimitCapacity <= userCountOfMeeting)
                    return OperationResult<bool>.OnFailed("Cannot Join On Meeting Because of Meeting Capacity is Fulled.");

                return OperationResult<bool>.OnSuccess(true);
            }
            catch (Exception exception)
            {
                return OperationResult<bool>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<long>> JoinUserOnMeeting(long userId, string meetingId)
        {
            try
            {
                var user = await _users
                    .GetUser(userId);
                if (user is null)
                    return OperationResult<long>.OnFailed("User is Not Valid.");

                var meeting = await _meetings
                    .FindByMeetingIdAsync(meetingId);
                if (meeting is null)
                    return OperationResult<long>.OnFailed("Meeting is Not Valid.");

                var result = await _memberShips
                    .CreateMemeberShipForMeetingAsync(meeting, user);
                await _unitOfWork.SaveChangesAsync();

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
                    return OperationResult.OnFailed("User is Not Valid.");
                var meeting = await _meetings.FindByMeetingIdAsync(meetingId);
                if (meeting is null)
                    return OperationResult.OnFailed("Meeting is Not Valid.");

                var member =  await _memberShips.GetMemberShipAsync(user.Id, meeting.Id);
                if (member is null)
                    return OperationResult.OnFailed("Your Considered Joining Was Not Found.");

                await member.BanUser();
                await _unitOfWork.SaveChangesAsync();

                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult.OnException(exception);
            }
        }
    }
}
