using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using BigBlueButtonAPI.Core;
using LIMS.Application.DTOs;
using LIMS.Application.Models;
using LIMS.Application.Models.Http.BBB;
using LIMS.Domain.Entities;


namespace LIMS.Infrastructure.Services.Api.BBB
{
    public class BBBConnectionService
    {
        private readonly BigBlueButtonAPIClient _client;

        public BBBConnectionService(BigBlueButtonAPIClient client)
            => _client = client;

        public async ValueTask<OperationResult<MeetingAddDto>> CreateMeetingOnBigBlueButton(CreateMeetingRequestModel createMeetingRequest)
        {
            try
            {
                var meetingCreateRequest = new CreateMeetingRequest
                {
                    name = createMeetingRequest.Name,
                    meetingID = createMeetingRequest.MeetingId,
                    record = createMeetingRequest.MustRecord,
                    moderatorPW = createMeetingRequest.ModeratorPassword,
                    attendeePW = createMeetingRequest.AttendeePassword,
                    welcome = $"Welcome to {createMeetingRequest.Name}"
                };

                var createMeetingResponse = await _client.CreateMeetingAsync(meetingCreateRequest);

                if (createMeetingResponse.Returncode == Returncode.Failed)
                    return OperationResult<MeetingAddDto>.OnFailed("A Problem Has Been Occurred in Creating Meet.");
                else
                    return OperationResult<MeetingAddDto>.OnSuccess(
                        new MeetingAddDto(
                        createMeetingResponse.meetingID,
                        meetingCreateRequest.record is true ? true : false,
                        meetingCreateRequest.name,
                        createMeetingResponse.moderatorPW,
                        createMeetingResponse.attendeePW
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
                _client.UseServerSettings(
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

        public async ValueTask<OperationResult> GetMeetingInformations(string meetingId)
        {
            try
            {
                var meetingInfoRequest = new GetMeetingInfoRequest { meetingID = meetingId };
                var resultInformation = await _client.GetMeetingInfoAsync(meetingInfoRequest);
                if (resultInformation.Returncode == Returncode.Failed)
                    return OperationResult.OnFailed(resultInformation.Message);
                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult.OnException(exception);
            }
        
        }
    }
}
