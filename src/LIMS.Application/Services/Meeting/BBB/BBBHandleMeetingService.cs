using LIMS.Application.Services.Database.BBB;
using LIMS.Application.DTOs;
using BigBlueButtonAPI.Core;
using LIMS.Application.Models;
using LIMS.Application.Models.Http.BBB;
using LIMS.Application.Mappers;

namespace LIMS.Application.Services.Meeting.BBB
{
    #region Main Services

    public class BbbHandleMeetingService
    {
        private readonly BbbMemberShipServiceImpl _memberShipService;
        private readonly BigBlueButtonAPIClient _client;
        private readonly BbbMeetingServiceImpl _meetingService;
        private readonly BbbServerServiceImpl _serverService;
        private readonly BbbUserServiceImpl _userService;

        public BbbHandleMeetingService(
            BigBlueButtonAPIClient client,
            BbbUserServiceImpl userService,
            BbbServerServiceImpl serverService,
            BbbMeetingServiceImpl sessionService,
            BbbMemberShipServiceImpl memberShipService
        ) =>
            (_userService, _client, _meetingService, _serverService, _memberShipService) = (
                userService,
                client,
                sessionService,
                serverService,
                memberShipService
            );

        #endregion

        /// <summary>
        /// Find Capable Server For Creating Meeting
        /// </summary>
        /// <returns>await of Single Response With Server DTO</returns>
        public async Task<SingleResponse<ServerAddEditDto>> UseMostCapableAndActiveServer()
        {
            /* Use CapableServer Service of Database Service */
            var server = await _serverService.MostCapableServer();

            if (!server.Success)
                return SingleResponse<ServerAddEditDto>.OnFailed(server.OnFailedMessage);

            do
            {
                var serverIsDown = await _serverService.UpdateServerCheckForBeingDown(server.Result.ServerUrl);

                if (!serverIsDown.Success)
                    return SingleResponse<ServerAddEditDto>.OnFailed(serverIsDown.OnFailedMessage);

                if (!serverIsDown.Result)
                    break;

                server = await _serverService.MostCapableServer();

            } while (server.Result.IsActive);

            return SingleResponse<ServerAddEditDto>.OnSuccess(server.Result);
        }

        /// <summary>
        /// Create A Meeting On Database
        /// </summary>
        /// <param name="meeting"></param>
        /// <returns>Meeting ID of Meeting</returns>
        public async ValueTask<SingleResponse<string>> CreateMeetingOnDatabase(MeetingAddDto meeting)
        {
            /* Use CreateNewMeeting Service of Database Service */
            var createMeeting = await _meetingService.CreateNewMeeting(meeting);

            if (!createMeeting.Success)
                return SingleResponse<string>.OnFailed(createMeeting.OnFailedMessage);

            return SingleResponse<string>.OnSuccess(createMeeting.Result);
        }

        /// <summary>
        /// Check BBB Settings are Ok or not
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns>Bool</returns>
        public async ValueTask<SingleResponse<bool>> IsBigBlueButtonOk(string meetingId)
        {
            try
            {
                /* Use IsRunning Service of BBB Api Service */
                var result = await _client.IsMeetingRunningAsync(
                    new IsMeetingRunningRequest
                    { meetingID = meetingId });

                if (result.Returncode is Returncode.Failed)
                    return SingleResponse<bool>.OnFailed(result.Message);

                return SingleResponse<bool>.OnSuccess(true);
            }
            catch (Exception exception)
            {
                return SingleResponse<bool>.OnFailed(exception.Message);
            }
        }

        /// <summary>
        /// Check and Handle Joining of an User
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="joinRequest"></param>
        /// <returns>Bool</returns>
        public async ValueTask<SingleResponse<bool>> CanJoinOnMeetingHandler(string meetingId, JoinMeetingRequestModel joinRequest)
        {
            /* All Flows of App For Joining Check */
            var server = await _meetingService.FindOneMeetingWithMeetingId(meetingId);
            if (!server.Success)
                return SingleResponse<bool>.OnFailed(server.OnFailedMessage);

            var meeting = await _meetingService.FindOneMeetingWithMeetingId(meetingId);
            if (!meeting.Success)
                return SingleResponse<bool>.OnFailed(meeting.OnFailedMessage);

            var canJoinOnMeeting = await _memberShipService.CanJoinUserOnMeeting(meeting.Result.Id);
            if (!canJoinOnMeeting.Success)
                return SingleResponse<bool>.OnFailed(server.OnFailedMessage);
            if (!canJoinOnMeeting.Result)
                return SingleResponse<bool>.OnFailed("Joining into This Class Not Accessed.");

            var canJoinOnServer = await _serverService.CanJoinServer(server.Result.Id);
            if (!canJoinOnServer.Success)
                return SingleResponse<bool>.OnFailed(server.OnFailedMessage);
            if (!canJoinOnServer.Result)
                return SingleResponse<bool>.OnFailed("Server Capacity is Fulled.");

            var user = await _userService.GetUserById(joinRequest.UserId);

            var canLoginOnMeeting = _meetingService.CanLoginOnExistMeeting(meetingId, UserDtoMapper.Map(user.Result), joinRequest.MeetingPassword).Result;

            if (!canLoginOnMeeting.Success)
                return SingleResponse<bool>.OnFailed(server.OnFailedMessage);
            if (!canLoginOnMeeting.Result)
                return SingleResponse<bool>.OnFailed("Your Credentials is not Exist in our Records.");

            return SingleResponse<bool>.OnSuccess(true);
        }

        /// <summary>
        /// Absolute Join On Meeting
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="meetingId"></param>
        /// <returns>Id of MemberShip</returns>
        public async ValueTask<SingleResponse<long>> JoiningOnMeetingOnDatabase(long userId, string meetingId)
        {
            var joinUserOnMeeting = await _memberShipService.JoinUserOnMeeting(userId, meetingId);

            if (!joinUserOnMeeting.Success)
                return SingleResponse<long>.OnFailed(joinUserOnMeeting.OnFailedMessage);

            return SingleResponse<long>.OnSuccess(joinUserOnMeeting.Result);
        }

        /// <summary>
        /// Handle Ending A Meeting On Database
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        public async Task<SingleResponse<string>> EndMeetingHandlerOnDatabase(string meetingId, DateTime now)
        {
            var endMeeting = await _meetingService.StopRunningMeeting(meetingId, now);

            if (!endMeeting.Success)
                return SingleResponse<string>.OnFailed(endMeeting.OnFailedMessage);

            return SingleResponse<string>.OnSuccess("Meeting is End.");
        }
    }
}
