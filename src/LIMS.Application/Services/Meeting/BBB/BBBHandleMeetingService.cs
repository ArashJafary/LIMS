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
using LIMS.Domain.Entity;
using LIMS.Application.Models.Http.BBB;

namespace LIMS.Application.Services.Meeting.BBB
{
    public class BBBHandleMeetingService
    {
        private readonly BigBlueButtonAPIClient _client;
        private readonly BBBMeetingServiceImpl _meetingService;
        private readonly BBBServerServiceImpl _serverService;
        private readonly BBBMemberShipServiceImpl _memberShipService;

        public BBBHandleMeetingService(
            BigBlueButtonAPIClient client,
            BBBMeetingServiceImpl sessionService,
            BBBServerServiceImpl serverService,
            BBBUserServiceImpl userService,
            BBBMemberShipServiceImpl memberShipService
        ) =>
            (_client, _meetingService, _serverService, _memberShipService) = (
                client,
                sessionService,
                serverService,
                memberShipService
            );

        public async Task<SingleResponse<ServerAddEditDto>> UseCapableServerCreateMeeting()
        {
            var server = await _serverService.MostCapableServer();

            if (!server.Success)
                if (server.Exception is not null)
                    return SingleResponse<ServerAddEditDto>.OnFailed(server.Exception.Data.ToString());
                else
                    return SingleResponse<ServerAddEditDto>.OnFailed(server.OnFailedMessage);
            else
               return SingleResponse<ServerAddEditDto>.OnSuccess(server.Result);
        }

        public async ValueTask<SingleResponse<string>> HandleCreateMeeting(MeetingAddDto meeting)
        {
            var createMeeting = await _meetingService.CreateNewMeetingAsync(meeting);

            if (!createMeeting.Success)
                if (createMeeting.Exception is not null)
                    return SingleResponse<string>.OnFailed(createMeeting.Exception.Data.ToString());
                else
                    return SingleResponse<string>.OnFailed(createMeeting.OnFailedMessage);
            else
                return SingleResponse<string>.OnSuccess(createMeeting.Result);
        }

        public async ValueTask<SingleResponse<bool>> IsBigBlueButtonOk(string meetingId)
        {
            try
            {
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

        public async ValueTask<SingleResponse<bool>> CanJoinOnMeetingHandler(long id,JoinMeetingRequestModel joinRequest)
        {
            var server = await _meetingService
                .FindMeetingWithMeetingId(joinRequest.MeetingId);
            if (!server.Success)
                if (server.Exception is not null)
                    return SingleResponse<bool>.OnFailed(server.Exception.Data.ToString());
                else
                    return SingleResponse<bool>.OnFailed(server.OnFailedMessage);

            var canJoinOnMeeting = await _memberShipService.CanJoinUserOnMeetingAsync(id);
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

            var cnaLoginOnMeeting = _meetingService.CanLoginOnExistMeeting(joinRequest.MeetingId, joinRequest.UserInformations.Role, joinRequest.MeetingPassword).Result;
            if (!cnaLoginOnMeeting.Success)
                if (cnaLoginOnMeeting.Exception is not null)
                    return SingleResponse<bool>.OnFailed(server.Exception.Data.ToString());
                else
                    return SingleResponse<bool>.OnFailed(server.OnFailedMessage);
            if (!cnaLoginOnMeeting.Result)
                return SingleResponse<bool>.OnFailed("Your Credentials is not Exist in our Records.");

            return SingleResponse<bool>.OnSuccess(true);
        }

        public async ValueTask<SingleResponse<long>> JoiningOnMeeting(long userId, string meetingId)
        {
            var joinUserOnMeeting = await _memberShipService.JoinUserAsync(userId, meetingId);
            if (!joinUserOnMeeting.Success)
                if (joinUserOnMeeting.Exception is not null)
                    return SingleResponse<long>.OnFailed(joinUserOnMeeting.Exception.Data.ToString());
                else
                    return SingleResponse<long>.OnFailed(joinUserOnMeeting.OnFailedMessage);
            return SingleResponse<long>.OnSuccess(joinUserOnMeeting.Result);
        }

        public async Task<SingleResponse<string>> EndMeetingHandler(string meetingId)
        {
            var endMeeting = await _meetingService.StopRunning(meetingId);
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
