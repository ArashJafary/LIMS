using LIMS.Application.DTOs;
using BigBlueButtonAPI.Core;
using LIMS.Application.Models;
using LIMS.Application.Mappers;
using LIMS.Application.Services.Database;
using LIMS.Domain.Services;
using LIMS.Application.Models.Http;

namespace LIMS.Application.Services.Handlers
{
    #region Main Services

    public class BbbHandleMeetingService : IHandleMeetingService
    {
        private readonly UserServiceImpl _userService;
        private readonly BigBlueButtonAPIClient _client;
        private readonly ServerServiceImpl _serverService;
        private readonly MeetingServiceImpl _meetingService;
        private readonly MemberShipServiceImpl _memberShipService;

        string IHandleMeetingService.MeetingHandlerName => "bbb";

        public BbbHandleMeetingService(
            BigBlueButtonAPIClient client,
            UserServiceImpl userService,
            ServerServiceImpl serverService,
            MeetingServiceImpl sessionService,
            MemberShipServiceImpl memberShipService
        ) =>
            (_userService, _client, _meetingService, _serverService, _memberShipService) = (
                userService,
                client,
                sessionService,
                serverService,
                memberShipService
            );

        #endregion

        public async Task<ResultSingleResponse<ServerAddEditDto>> UseMostCapableAndActiveServer()
        {
            var server = await _serverService.GetMostCapableServer();

            if (!server.Success)
                return ResultSingleResponse<ServerAddEditDto>.OnFailed(server.OnFailedMessage);

            do
            {
                var serverIsDown = await _serverService.UpdateServerCheckForBeingDown(server.Result.ServerUrl);

                if (!serverIsDown.Success)
                    return ResultSingleResponse<ServerAddEditDto>.OnFailed(serverIsDown.OnFailedMessage);

                if (!serverIsDown.Result)
                    break;

                server = await _serverService.GetMostCapableServer();

            } while (server.Result.IsActive);

            return ResultSingleResponse<ServerAddEditDto>.OnSuccess(server.Result);
        }

        public async ValueTask<ResultSingleResponse<string>> CreateMeetingOnDatabase(MeetingAddDto meeting)
        {
            var createMeeting = await _meetingService.CreateNewMeeting(meeting);

            if (!createMeeting.Success)
                return ResultSingleResponse<string>.OnFailed(createMeeting.OnFailedMessage);

            return ResultSingleResponse<string>.OnSuccess(createMeeting.Result);
        }

        public async ValueTask<ResultSingleResponse<bool>> IsOkSettings(string meetingId)
        {
            try
            {
                var result = await _client.IsMeetingRunningAsync(
                    new IsMeetingRunningRequest
                    { meetingID = meetingId });

                if (result.Returncode is Returncode.Failed)
                    return ResultSingleResponse<bool>.OnFailed(result.Message);

                return ResultSingleResponse<bool>.OnSuccess(true);
            }
            catch (Exception exception)
            {
                return ResultSingleResponse<bool>.OnFailed(exception.Message);
            }
        }

        public async ValueTask<ResultSingleResponse<bool>> CanJoinOnMeetingHandler(string meetingId, BbbJoinMeetingRequestModel joinRequest)
        {
            var server = await _meetingService.FindOneMeetingWithMeetingId(meetingId);
            if (!server.Success)
                return ResultSingleResponse<bool>.OnFailed(server.OnFailedMessage);

            var meeting = await _meetingService.FindOneMeetingWithMeetingId(meetingId);
            if (!meeting.Success)
                return ResultSingleResponse<bool>.OnFailed(meeting.OnFailedMessage);

            var canJoinOnMeeting = await _memberShipService.CanJoinUserOnMeeting(meeting.Result.Id);
            if (!canJoinOnMeeting.Success)
                return ResultSingleResponse<bool>.OnFailed(server.OnFailedMessage);
            if (!canJoinOnMeeting.Result)
                return ResultSingleResponse<bool>.OnFailed("Joining into This Class Not Accessed.");

            var canJoinOnServer = await _serverService.CanJoinServer(server.Result.Id);
            if (!canJoinOnServer.Success)
                return ResultSingleResponse<bool>.OnFailed(server.OnFailedMessage);
            if (!canJoinOnServer.Result)
                return ResultSingleResponse<bool>.OnFailed("Server Capacity is Fulled.");

            var user = await _userService.GetUserById(joinRequest.UserId);

            var canLoginOnMeeting = _meetingService.CanLoginOnExistMeeting(meetingId, UserDtoMapper.Map(user.Result), joinRequest.MeetingPassword).Result;

            if (!canLoginOnMeeting.Success)
                return ResultSingleResponse<bool>.OnFailed(server.OnFailedMessage);
            if (!canLoginOnMeeting.Result)
                return ResultSingleResponse<bool>.OnFailed("Your Credentials is not Exist in our Records.");

            return ResultSingleResponse<bool>.OnSuccess(true);
        }

        public async ValueTask<ResultSingleResponse<long>> JoiningOnMeetingOnDatabase(long userId, string meetingId)
        {
            var joinUserOnMeeting = await _memberShipService.JoinUserOnMeeting(userId, meetingId);

            if (!joinUserOnMeeting.Success)
                return ResultSingleResponse<long>.OnFailed(joinUserOnMeeting.OnFailedMessage);

            return ResultSingleResponse<long>.OnSuccess(joinUserOnMeeting.Result);
        }

        public async Task<ResultSingleResponse<string>> EndMeetingHandlerOnDatabase(string meetingId, DateTime now)
        {
            var endMeeting = await _meetingService.StopRunningMeeting(meetingId, now);

            if (!endMeeting.Success)
                return ResultSingleResponse<string>.OnFailed(endMeeting.OnFailedMessage);

            return ResultSingleResponse<string>.OnSuccess("Meeting is End.");
        }
    }
}
