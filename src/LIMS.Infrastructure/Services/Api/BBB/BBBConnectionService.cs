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
using LIMS.Domain;
using LIMS.Domain.Entities;


namespace LIMS.Infrastructure.Services.Api.BBB
{
    public class BBBConnectionService
    {
        private readonly BigBlueButtonAPIClient _bbbClient;

        public BBBConnectionService(BigBlueButtonAPIClient bbbClient)
            => _bbbClient = bbbClient;

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
                        meetingRequestModel.ParentId,
                        meetingRequestModel.Name,
                        meetingRequestModel.ModeratorPassword,
                        meetingRequestModel.AttendeePassword,
                        meetingRequestModel.StartDateTime,
                        meetingRequestModel.LimitCapacity,
                        meetingRequestModel.Server
                    ));
            }
            catch (Exception exception)
            {
                return OperationResult<MeetingAddDto>.OnException(exception);
            }
        }

        public async Task<OperationResult> ChangeServerSettings(BigBlueButtonAPISettings settings,ServerAddEditDto server)
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

        public async Task<OperationResult<string>> JoiningOnMeeting(JoinMeetingRequestModel joinOnMeetingRequest)
        {
            try
            {
                var joinOnMeetingRequestJoin = new JoinMeetingRequest { meetingID = joinOnMeetingRequest.MeetingId };

                if (joinOnMeetingRequest.UserInformations.Role == UserRoleTypes.Moderator)
                {
                    joinOnMeetingRequestJoin.password = joinOnMeetingRequest.MeetingPassword;
                    joinOnMeetingRequestJoin.userID = "1";
                    joinOnMeetingRequestJoin.fullName = joinOnMeetingRequest.UserInformations.FullName;
                }
                else if (joinOnMeetingRequest.UserInformations.Role == UserRoleTypes.Attendee)
                {
                    joinOnMeetingRequestJoin.password = joinOnMeetingRequest.MeetingPassword;
                    joinOnMeetingRequestJoin.userID = "2";
                    joinOnMeetingRequestJoin.fullName = joinOnMeetingRequest.UserInformations.FullName;
                }
                else
                {
                    joinOnMeetingRequestJoin.guest = true;
                    joinOnMeetingRequestJoin.fullName = joinOnMeetingRequest.UserInformations.FullName;
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

        public async Task<OperationResult> EndExistMeeting(string meetingId,string moderatorPassword)
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
