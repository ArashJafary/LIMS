using LIMS.Application.Services.Database.BBB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using LIMS.Application.DTOs;
using BigBlueButtonAPI.Core;
using LIMS.Application.DTOs;
using LIMS.Application.Models;
using LIMS.Domain.Entities;
using LIMS.Application.Models.Http.BBB;
using LIMS.Application.Mappers;
using LIMS.Domain.IRepositories;

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
        private readonly IServerRepository _serverRepository;

        public BbbHandleMeetingService(
            BigBlueButtonAPIClient client,
            BbbUserServiceImpl userService,
            IServerRepository serverRepository,
            BbbServerServiceImpl serverService,
            BbbMeetingServiceImpl sessionService,
            BbbMemberShipServiceImpl memberShipService
        ) =>
            (_userService, _client, _meetingService, _serverService, _memberShipService, _serverRepository) = (
                userService,
                client,
                sessionService,
                serverService,
                memberShipService,
                serverRepository
            );

        #endregion
        /// <summary>
        /// Find Capable Server For Creating Meeting
        /// </summary>
        /// <returns>await of Single Response With Server DTO</returns>
        public async Task<SingleResponse<ServerAddEditDto>> UseMostCapableAndActiveServer()
        {
            /* Use CapableServer Service of Database Service */
            var server = await _serverService
                .MostCapableServer();

            if (!server.Success)
                return server.Exception is null
                    ? SingleResponse<ServerAddEditDto>.OnFailed(server.Exception.Data.ToString())
                    : SingleResponse<ServerAddEditDto>.OnFailed(server.OnFailedMessage);

            while (!server.Result.IsActive)
            {
                var serverIsDown = await _serverService
                    .UpdateServerForBeingDown(server.Result.ServerUrl);

                if (!serverIsDown.Success)
                    return serverIsDown.Exception is null
                        ? SingleResponse<ServerAddEditDto>.OnFailed(serverIsDown.OnFailedMessage)
                        : SingleResponse<ServerAddEditDto>.OnFailed(serverIsDown.Exception.Message);

                if (serverIsDown.Result)
                    server = await _serverService
                        .MostCapableServer();
                else
                    break;
            }

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
                if (createMeeting.Exception is not null)
                    return SingleResponse<string>.OnFailed(createMeeting.Exception.Data.ToString());
                else
                    return SingleResponse<string>.OnFailed(createMeeting.OnFailedMessage);
            else
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
                    { meetingID = meetingId }
                );

                if (result.Returncode == Returncode.Failed)
                    return SingleResponse<bool>.OnFailed(result.Message);
                else
                    return SingleResponse<bool>.OnSuccess(true);
            }
            catch (Exception exception)
            {
                return SingleResponse<bool>.OnFailed(exception.Data.ToString());
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
            var server = await _meetingService
                .FindOneMeetingWithMeetingId(meetingId);
            if (!server.Success)
                if (server.Exception is not null)
                    return SingleResponse<bool>.OnFailed(server.Exception.Data.ToString());
                else
                    return SingleResponse<bool>.OnFailed(server.OnFailedMessage);

            var meeting = await _meetingService.FindOneMeetingWithMeetingId(meetingId);
            if (!meeting.Success)
                if (meeting.Exception is not null)
                    return SingleResponse<bool>.OnFailed(meeting.Exception.Data.ToString());
                else
                    return SingleResponse<bool>.OnFailed(meeting.OnFailedMessage);

            var canJoinOnMeeting = await _memberShipService.CanJoinUserOnMeeting(meeting.Result.Id);
            if (!canJoinOnMeeting.Success)
                if (server.Exception is not null)
                    return SingleResponse<bool>.OnFailed(canJoinOnMeeting.Exception.Data.ToString());
                else
                    return SingleResponse<bool>.OnFailed(server.OnFailedMessage);
            if (!canJoinOnMeeting.Result)
                return SingleResponse<bool>.OnFailed("Joining into This Class Not Accessed.");

            var canJoinOnServer = await _serverService.CanJoinServer(server.Result.Id);
            if (!canJoinOnServer.Success)
                if (canJoinOnServer.Exception is not null)
                    return SingleResponse<bool>.OnFailed(server.Exception.Data.ToString());
                else
                    return SingleResponse<bool>.OnFailed(server.OnFailedMessage);
            if (!canJoinOnServer.Result)
                return SingleResponse<bool>.OnFailed("Server Capacity is Fulled.");


            var user = await _userService.GetUser(joinRequest.UserId);

            var cnaLoginOnMeeting = _meetingService
                .CanLoginOnExistMeeting(meetingId, UserDtoMapper.Map(user.Result), joinRequest.MeetingPassword).Result;

            if (!cnaLoginOnMeeting.Success)
                if (cnaLoginOnMeeting.Exception is not null)
                    return SingleResponse<bool>.OnFailed(server.Exception.Data.ToString());
                else
                    return SingleResponse<bool>.OnFailed(server.OnFailedMessage);

            if (!cnaLoginOnMeeting.Result)
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
                if (joinUserOnMeeting.Exception is not null)
                    return SingleResponse<long>.OnFailed(joinUserOnMeeting.Exception.Data.ToString());
                else
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
            var endMeeting = await _meetingService
                .StopRunningMeeting(meetingId, now);

            if (!endMeeting.Success)
                if (endMeeting.Exception is not null)
                    return SingleResponse<string>.OnFailed(endMeeting.Exception.Data.ToString());
                else
                    return SingleResponse<string>.OnFailed(endMeeting.OnFailedMessage);

            else
                return SingleResponse<string>.OnSuccess("Meeting is End.");
        }
    }
}
