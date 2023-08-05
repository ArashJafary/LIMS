using LIMS.Application.DTOs;
using LIMS.Application.Exceptions.Http.BBB;
using LIMS.Application.Mappers;
using LIMS.Domain;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Application.Models;
using LIMS.Domain.Enumerables;
using Microsoft.Extensions.Logging;

namespace LIMS.Application.Services.Database
{
    public class MeetingServiceImpl
    {
        private readonly IMeetingRepository _meetings;
        private readonly ILogger<MeetingServiceImpl> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemberShipRepository _memberShips;

        public MeetingServiceImpl(IMeetingRepository meetings, IUnitOfWork unitOfWork, ILogger<MeetingServiceImpl> logger) =>
            (_meetings, _unitOfWork, _logger
            ) = (meetings, unitOfWork, logger);

        public async ValueTask<OperationResult<string>> CreateNewMeeting(MeetingAddDto meeting)
        {
            try
            {
                var result = await _meetings
                    .CreateAsync(await MeetingDtoMapper.Map(meeting));

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"{meeting.Name} is Created Successfully");

                return OperationResult<string>.OnSuccess(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<string>.OnException(exception);
            }
        }

        private async ValueTask<OperationResult> CheckUserRolesForLoginOnMeeting(User user, Domain.Entities.Meeting meeting, string password)
        {
            try
            {
                if (user.Role == UserRoleTypes.Attendee)
                    if (meeting.AttendeePassword == password)
                        return OperationResult<bool>.OnSuccess(true);

                    else if (user.Role == UserRoleTypes.Moderator)
                        if (meeting.ModeratorPassword == password)
                            return OperationResult<bool>.OnSuccess(true);

                        else if (user.Role == UserRoleTypes.Guest)
                            return OperationResult<bool>.OnSuccess(true);

                _logger.LogWarning($"{user.FullName} Login Proccess as {user.Role} Failed Because of Incorrect Password.");

                return OperationResult<bool>.OnFailed(
                    "Your Moderator User or Password Intended Not Exists in my Records.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<bool>.OnException(exception);
            }
        }

        private async ValueTask<OperationResult<bool>> CheckUserBanned(long userId, long meetingId)
        {
            try
            {
                var memberShip = await _memberShips.GetAsync(userId, meetingId);

                if (memberShip.UserRejected)
                    return OperationResult<bool>.OnSuccess(true);
                else
                {
                    _logger.LogInformation($"{memberShip.User.FullName} is Banned And Cannot Login");

                    return OperationResult<bool>.OnSuccess(false);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<bool>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<bool>> CanLoginOnExistMeeting(string meetingId, User user, string password)
        {
            try
            {
                var meeting = await _meetings.GetByMeetingIdAsync(meetingId);

                var checkBannedOrNot = await CheckUserBanned(user.Id, meeting.Id);

                if (!checkBannedOrNot.Success)
                    return OperationResult<bool>.OnFailed(checkBannedOrNot.OnFailedMessage);

                if (checkBannedOrNot.Result)
                    return OperationResult<bool>.OnFailed("User Is Banned And Cannot Login.");

                var isPrivate = meeting.Type == MeetingTypes.Private;

                var checkUserRole = await CheckUserRolesForLoginOnMeeting(user, meeting, password);

                if (isPrivate)
                    foreach (var meetingUser in meeting.Users)
                        if (meetingUser.Id == user.Id)
                            if (!checkUserRole.Success)
                                return OperationResult<bool>.OnFailed(checkUserRole.OnFailedMessage);
                            else
                                break;
                        else
                            return OperationResult<bool>.OnFailed("This Meeting is Private and You Cant Join.");
                else
                    if (!checkUserRole.Success)
                    return OperationResult<bool>.OnFailed(checkUserRole.OnFailedMessage);

                return OperationResult<bool>.OnSuccess(true);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<bool>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<Domain.Entities.Meeting>> FindOneMeeting(long id)
        {
            try
            {
                var meeting = await _meetings.GetAsync(id);

                if (meeting is null)
                    return OperationResult<Domain.Entities.Meeting>.OnFailed("Meeting Information is Null");

                return OperationResult<Domain.Entities.Meeting>.OnSuccess(meeting);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<Domain.Entities.Meeting>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<Domain.Entities.Meeting>> FindOneMeetingWithMeetingId(string meetingId)
        {
            try
            {
                var meeting = await _meetings.GetByMeetingIdAsync(meetingId);

                if (meeting is null)
                    return OperationResult<Domain.Entities.Meeting>.OnFailed("Meeting Information is Null");

                return OperationResult<Domain.Entities.Meeting>.OnSuccess(meeting);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<Domain.Entities.Meeting>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult> UpdateExistedMeeting(long id, MeetingEditDto meetingInput)
        {
            try
            {
                var meeting = await _meetings.GetAsync(id);

                meeting.Update(meetingInput.Name,
                    meetingInput.ModeratorPassword,
                    meetingInput.AttendeePassword,
                    meetingInput.limitCapacity);

                await _unitOfWork.SaveChangesAsync();

                return new OperationResult();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult.OnException(exception);
            }
        }

        public async ValueTask<OperationResult> StopRunningMeeting(string meetingId, DateTime now)
        {
            try
            {
                var meeting = await _meetings.GetByMeetingIdAsync(meetingId);

                meeting.EndSession(now);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Meeting {meeting.Name} Stoped.");

                return new OperationResult();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult.OnException(exception);
            }
        }
    }
}
