using BigBlueButtonAPI.Core;
using LIMS.Application.DTOs;
using LIMS.Application.Models;
using LIMS.Application.Models.Http;
using LIMS.Application.Services.Database;
using LIMS.Application.Services.Interfaces;
using LIMS.Domain;
using LIMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace LIMS.Infrastructure.ExternalApi.BBB
{
    public class BbbConnectionService : IConnectionService
    {
        private readonly BigBlueButtonAPIClient _bbbClient;
        private readonly ILogger<BbbConnectionService> _logger;
        private readonly UserServiceImpl _userService;

        public BbbConnectionService(BigBlueButtonAPIClient bbbClient,
            UserServiceImpl userService,
            ILogger<BbbConnectionService> logger)
            => (_bbbClient, _userService, _logger) = (bbbClient, userService, logger);

        public async ValueTask<OperationResult<MeetingAddDto>> CreateMeetingOnPlatform(MeetingAddDto meetingRequestModel)
        {
            try
            {
                var meetingCreateRequest = new CreateMeetingRequest
                {
                    name = meetingRequestModel.Name,
                    meetingID = meetingRequestModel.MeetingId,
                    record = meetingRequestModel.IsRecord,
                    moderatorPW = meetingRequestModel.ModeratorPassword,
                    attendeePW = meetingRequestModel.AttendeePassword,
                    welcome = $"Welcome to {meetingRequestModel.Name}",
                    isBreakout = meetingRequestModel.IsBreakout,
                    freeJoin = meetingRequestModel.CanFreeJoinOnBreakout,
                };

                var createMeetingResponse = await _bbbClient.CreateMeetingAsync(meetingCreateRequest);

                if (createMeetingResponse.Returncode == Returncode.Failed)
                    return OperationResult<MeetingAddDto>.OnFailed("A Problem Has Been Occurred in Creating Meet.");

                return OperationResult<MeetingAddDto>.OnSuccess(
                    new MeetingAddDto(
                    meetingRequestModel.MeetingId,
                    meetingRequestModel.IsRecord,
                    meetingRequestModel.IsBreakout,
                    meetingRequestModel.CanFreeJoinOnBreakout,
                    meetingCreateRequest.parentMeetingID,
                    meetingRequestModel.Name,
                    meetingRequestModel.ModeratorPassword,
                    meetingRequestModel.AttendeePassword,
                    meetingRequestModel.StartDateTime,
                    meetingRequestModel.EndDateTime,
                    meetingRequestModel.LimitCapacity,
                    meetingRequestModel.Server,
                    meetingRequestModel.AutoStartRecord,
                    meetingRequestModel.Platform));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<MeetingAddDto>.OnException(exception);
            }
        }

        public async Task<OperationResult> ChangeServerSettings(ServerAddEditDto server)
        {
            try
            {
                await _bbbClient.UseServerSettings(
                    new BigBlueButtonAPISettings
                    {
                        SharedSecret = server.ServerSecret,
                        ServerAPIUrl = server.ServerUrl
                    });

                return new OperationResult();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<MeetingAddDto>.OnException(exception);
            }
        }

        public async Task<OperationResult<string>> JoiningOnMeeting(string meetingId, BbbJoinMeetingRequestModel joinOnMeetingRequest)
        {
            try
            {
                var meeting = await _bbbClient.GetMeetingInfoAsync(new GetMeetingInfoRequest { meetingID = meetingId });

                var user = await _userService.GetUserById(joinOnMeetingRequest.UserId);

                if (!user.Success)
                    return OperationResult<string>.OnFailed(user.OnFailedMessage);

                var joinOnMeetingRequestJoin = new JoinMeetingRequest { meetingID = meeting.meetingName };

                joinOnMeetingRequestJoin.fullName = user.Result.FullName;
                joinOnMeetingRequestJoin.userID = joinOnMeetingRequest.UserId.ToString();
                joinOnMeetingRequestJoin.password = joinOnMeetingRequest.MeetingPassword;

                if (user.Result.Role == UserRoleTypes.Moderator || user.Result.Role == UserRoleTypes.Attendee)
                    joinOnMeetingRequestJoin.guest = false;
                else if (user.Result.Role == UserRoleTypes.Guest)
                    joinOnMeetingRequestJoin.guest = true;
                else
                    throw new NotImplementedException("Undefined User Role Type.");

                var url = _bbbClient.GetJoinMeetingUrl(joinOnMeetingRequestJoin);

                if (url is null || string.IsNullOrEmpty(url))
                    return OperationResult<string>.OnFailed("A Problem in Joining On Meet.");

                _logger.LogInformation($"{user.Result.FullName} Joined on {meeting.meetingName} at {DateTime.Now}");

                return OperationResult<string>.OnSuccess(url);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<string>.OnException(exception);
            }
        }

        public async Task<OperationResult> EndExistMeeting(string meetingId, string moderatorPassword)
        {
            try
            {
                var endMeeting = await _bbbClient.EndMeetingAsync(
                    new EndMeetingRequest { meetingID = meetingId, password = moderatorPassword });

                if (endMeeting.Returncode == Returncode.Failed)
                    return OperationResult.OnFailed(endMeeting.Message);

                var meeting = _bbbClient.GetMeetingInfoAsync(new GetMeetingInfoRequest { meetingID = meetingId });

                _logger.LogInformation($"the Started Meeting : {meeting.Result.meetingName} is Stop and Finished at {DateTime.Now}.");

                return new OperationResult();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<string>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<GetMeetingInfoResponse>> GetMeetingInformations(string meetingId)
        {
            try
            {
                var getMeetingInformation = await _bbbClient.GetMeetingInfoAsync(
                    new GetMeetingInfoRequest { meetingID = meetingId });

                if (getMeetingInformation.Returncode == Returncode.Failed)
                    return OperationResult<GetMeetingInfoResponse>.OnFailed(getMeetingInformation.Message);

                return OperationResult<GetMeetingInfoResponse>.OnSuccess(getMeetingInformation);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<GetMeetingInfoResponse>.OnException(exception);
            }
        }
    }
}
