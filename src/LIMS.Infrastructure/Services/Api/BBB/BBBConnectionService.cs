using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using BigBlueButtonAPI.Core;
using LIMS.Application.DTOs;
using LIMS.Application.Models;
using LIMS.Application.Models;
using LIMS.Application.Models.Http.BBB;
using LIMS.Application.Services.Database.BBB;
using LIMS.Domain;
using LIMS.Domain.Entities;


namespace LIMS.Infrastructure.Services.Api.BBB
{
    public class BBBConnectionService
    {
        private readonly BigBlueButtonAPIClient _bbbClient;
        private readonly BBBUserServiceImpl _userService;

        public BBBConnectionService(BigBlueButtonAPIClient bbbClient, BBBUserServiceImpl userService)
            => (_bbbClient, _userService) = (bbbClient, userService);

        public async ValueTask<OperationResult<MeetingAddDto>> CreateMeetingOnBigBlueButton(MeetingAddDto meetingRequestModel)
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
                else
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
                        meetingRequestModel.Platform
                    ));
            }
            catch (Exception exception)
            {
                return OperationResult<MeetingAddDto>.OnException(exception);
            }
        }

        public async Task<OperationResult> ChangeServerSettings(BigBlueButtonAPISettings settings, ServerAddEditDto server)
        {
            try
            {
                await _bbbClient.UseServerSettings(
                    new BigBlueButtonAPISettings
                    {
                        SharedSecret = server.ServerSecret,
                        ServerAPIUrl = server.ServerUrl
                    }
                );
                return new OperationResult();

            }
            catch (Exception exception)
            {
                return OperationResult<MeetingAddDto>.OnException(exception);
            }
        }

        public async Task<OperationResult<string>> JoiningOnMeeting(string meetingId,JoinMeetingRequestModel joinOnMeetingRequest)
        {
            try
            {
                var user = await _userService.GetUser(joinOnMeetingRequest.UserId);
                if (!user.Success)
                    return user.Exception is null
                        ? OperationResult<string>.OnFailed(user.OnFailedMessage)
                        : throw new Exception("Cannot Find User.");

                var joinOnMeetingRequestJoin = new JoinMeetingRequest { meetingID = meetingId };

                if (user.Result.Role == UserRoleTypes.Moderator)
                {
                    joinOnMeetingRequestJoin.password = joinOnMeetingRequest.MeetingPassword;
                    joinOnMeetingRequestJoin.userID = "1";
                    joinOnMeetingRequestJoin.fullName = user.Result.FullName;
                }
                else if (user.Result.Role == UserRoleTypes.Attendee)
                {
                    joinOnMeetingRequestJoin.password = joinOnMeetingRequest.MeetingPassword;
                    joinOnMeetingRequestJoin.userID = "2";
                    joinOnMeetingRequestJoin.fullName = user.Result.FullName;
                }
                else
                {
                    joinOnMeetingRequestJoin.guest = true;
                    joinOnMeetingRequestJoin.fullName = user.Result.FullName;
                }

                var url = _bbbClient.GetJoinMeetingUrl(joinOnMeetingRequestJoin);
                if (url is null || string.IsNullOrEmpty(url))
                    return OperationResult<string>.OnFailed("A Problem in Joining On Meet.");

                return OperationResult<string>.OnSuccess(url);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.OnException(exception);
            }
        }

        public async Task<OperationResult> EndExistMeeting(string meetingId, string moderatorPassword)
        {
            try
            {
                var result = await _bbbClient.EndMeetingAsync(
                    new EndMeetingRequest { meetingID = meetingId, password = moderatorPassword }
                );
                if (result.Returncode == Returncode.Failed)
                    return OperationResult.OnFailed(result.Message);
                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult<string>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<GetMeetingInfoResponse>> GetMeetingInformations(string meetingId)
        {
            try
            {
                var result = await _bbbClient.GetMeetingInfoAsync(
                    new GetMeetingInfoRequest { meetingID = meetingId }
                );
                if (result.Returncode == Returncode.Failed)
                    return OperationResult<GetMeetingInfoResponse>.OnFailed(result.Message);

                return OperationResult<GetMeetingInfoResponse>.OnSuccess(result);
            }
            catch (Exception exception)
            {
                return OperationResult<GetMeetingInfoResponse>.OnException(exception);
            }
        }
    }
}
