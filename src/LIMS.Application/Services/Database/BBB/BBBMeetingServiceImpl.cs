using LIMS.Application.DTOs;
using LIMS.Application.Exceptions.Http.BBB;
using LIMS.Application.Mappers;
using LIMS.Domain;
using LIMS.Domain.IRepositories;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Application.Models;
using LIMS.Domain.Entities;
using LIMS.Domain;
using LIMS.Domain.Enumerables;

namespace LIMS.Application.Services.Database.BBB
{
    public class BBBMeetingServiceImpl
    {
        private readonly IMeetingRepository _meetings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _users;

        public BBBMeetingServiceImpl(IMeetingRepository meetings, IUnitOfWork unitOfWork, IUserRepository users) =>
            (_meetings,_unitOfWork,_users
            ) = (meetings, unitOfWork,users);

        public async ValueTask<OperationResult<string>> CreateNewMeeting(MeetingAddDto meeting)
        {
            try
            {
                var result = await _meetings
                    .CreateMeetingAsync(await MeetingDtoMapper.Map(meeting));

                await _unitOfWork
                    .SaveChangesAsync();

                return OperationResult<string>.OnSuccess(result);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.OnException(exception);
            }
        }

        private async ValueTask<OperationResult> CheckUserRolesForLoginOnMeeting(User user,Domain.Entities.Meeting meeting,string password)
        {
            if (user.Role.RoleName == UserRoleTypes.Attendee.ToString())
                if (meeting.AttendeePassword == password)
                    return OperationResult<bool>.OnSuccess(true);

                else if (user.Role.RoleName == UserRoleTypes.Moderator.ToString())
                    if (meeting.ModeratorPassword == password)
                        return OperationResult<bool>.OnSuccess(true);

                    else if (user.Role.RoleName == UserRoleTypes.Guest.ToString())
                        return OperationResult<bool>.OnSuccess(true);

            return OperationResult<bool>.OnFailed(
                "Your Moderator User or Password Intended Not Exists in my Records.");
        }

        public async ValueTask<OperationResult<bool>> CanLoginOnExistMeeting(string meetingId,User user, string password)
        {
            try
            {
                var meeting = await _meetings
                    .FindByMeetingIdAsync(meetingId);

                var isPrivate = meeting.Type == MeetingTypes.Private;

                var checkUserRole = await CheckUserRolesForLoginOnMeeting(user,
                    meeting, 
                    password);

                if (isPrivate)
                    foreach (var meetingUser in meeting.Users)
                        if (meetingUser.Id == user.Id)
                            if (!checkUserRole.Success)
                                return checkUserRole.Exception is null
                                    ? OperationResult<bool>.OnFailed(checkUserRole.OnFailedMessage)
                                    : throw new UserCannotLoginException(checkUserRole.Exception.Message);
                            else
                                break;
                        else
                            return OperationResult<bool>.OnFailed("This Meeting is Private and You Cant Join.");
                else
                    if (!checkUserRole.Success)
                        return checkUserRole.Exception is null
                            ? OperationResult<bool>.OnFailed(checkUserRole.OnFailedMessage)
                            : throw new UserCannotLoginException(checkUserRole.Exception.Message);

                return OperationResult<bool>.OnSuccess(true);
            }
            catch (Exception exception)
            {
                return OperationResult<bool>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<Domain.Entities.Meeting>> FindOneMeeting(long id)
        {
            try
            {
                var meeting  = await _meetings.FindAsync(id);
                if (meeting is null)
                    return OperationResult<Domain.Entities.Meeting>.OnFailed("Meeting Information is Null");

                return OperationResult<Domain.Entities.Meeting>.OnSuccess(meeting);
            }
            catch (Exception exception)
            {
                return OperationResult<Domain.Entities.Meeting>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<Domain.Entities.Meeting>> FindOneMeetingWithMeetingId(string meetingId)
        {
            try
            {
                var meeting = await _meetings.FindByMeetingIdAsync(meetingId);
                if (meeting is null)
                    return OperationResult<Domain.Entities.Meeting>.OnFailed("Meeting Information is Null");
                return OperationResult<Domain.Entities.Meeting>.OnSuccess(meeting);
            }
            catch (Exception exception)
            {
                return OperationResult<Domain.Entities.Meeting>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult> UpdateExistedMeeting(long id, MeetingEditDto meetingInput)
        {
            try
            {
                var meeting =await _meetings.FindAsync(id);
                await meeting.Update(meetingInput.Name,
                    meetingInput.ModeratorPassword,
                    meetingInput.AttendeePassword,
                    meetingInput.limitCapacity);
                await _unitOfWork
                    .SaveChangesAsync();
                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult.OnException(exception);
            }
        }

        public async ValueTask<OperationResult> StopRunningMeeting(string meetingId,DateTime now)
        {
            try
            {
                var meeting = await _meetings
                    .FindByMeetingIdAsync(meetingId);

                meeting.EndSession(now);

                await _unitOfWork
                    .SaveChangesAsync()
                    ;
                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult.OnException(exception);
            }
        }
    }
}
