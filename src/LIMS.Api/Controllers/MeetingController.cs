using LIMS.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using LIMS.Application.Services.Database;
using LIMS.Application.Models.Http;
using LIMS.Domain.Services;
using LIMS.Application.Services.Interfaces;
using LIMS.Application.Services.Http;

namespace LIMS.Api.Controllers
{

    [ApiController]
    [Route("api/Meeting")]
    public class MeetingController : ControllerBase
    {
        private readonly UserServiceImpl _userService;
        private readonly IConnectionService _connectionService;
        private readonly IHandleMeetingService _handleMeetingService;
        private readonly MeetingSettingsService _meetingSettingsService;

        public MeetingController(
            UserServiceImpl userService,
            MeetingSettingsService meetingSettingsService,
            IConnectionService connectionService,
            IHandleMeetingService handleMeetingService
        ) =>
        (
            _connectionService,
            _handleMeetingService,
            _userService,
            _meetingSettingsService
        ) = (
            connectionService,
            handleMeetingService,
            userService,
            meetingSettingsService
        );

        [HttpGet("[action]/{meeting}", Name = nameof(GetMeetingInformation))]
        public async ValueTask<IActionResult> GetMeetingInformation([FromRoute] string meeting)
        {
            var platformSettings = await _meetingSettingsService.IsSettingsOkAsync(meeting);
            if (!platformSettings.Success)
                return BadRequest($"{platformSettings.Platform} Settings Have Problem.");

            var getMeetingInformation = await _connectionService.GetMeetingInformations(meeting);
            if (!getMeetingInformation.Success)
                return BadRequest(getMeetingInformation.OnFailedMessage);

            return Ok(getMeetingInformation.Result);
        }

        [HttpPost("[action]", Name = nameof(CreateMeeting))]
        public async Task<IActionResult> CreateMeeting([FromBody] BbbCreateMeetingRequestModel createMeetingRequest)
        {
            var server = await _handleMeetingService.UseMostCapableAndActiveServer();

            await _connectionService.ChangeServerSettings(server.Data!);

            var createMeetingConnection = await _connectionService
                .CreateMeetingOnPlatform(
                    new MeetingAddDto(
                        createMeetingRequest.MeetingId,
                        createMeetingRequest.IsRecord,
                        createMeetingRequest.IsBreakout,
                        createMeetingRequest.CanFreeJoinOnBreakout,
                        createMeetingRequest.ParentId,
                        createMeetingRequest.Name,
                        createMeetingRequest.ModeratorPassword,
                        createMeetingRequest.AttendeePassword,
                        createMeetingRequest.StartDateTime,
                        createMeetingRequest.EndDateTime,
                        createMeetingRequest.LimitCapacity,
                        server.Data!,
                        createMeetingRequest.AutoStartRecord,
                        createMeetingRequest.Platform
                    ));

            var createMeeting = await _handleMeetingService.CreateMeetingOnDatabase(createMeetingConnection.Result);

            if (createMeeting.Data is null)
                return createMeeting.Errors.Count() > 1
                    ? BadRequest(createMeeting.Errors)
                    : BadRequest(createMeeting.Error);

            var getMeetingInformation = await _connectionService.GetMeetingInformations(createMeetingRequest.MeetingId);

            return Ok(getMeetingInformation.Result);
        }

        [HttpPost("[action]/{meetingId}", Name = nameof(JoinOnMeeting))]
        public async ValueTask<IActionResult> JoinOnMeeting(
            [FromRoute] string meetingId,
            [FromBody] BbbJoinMeetingRequestModel joinOnMeetingRequest
        )
        {
            var platformSettings = await _meetingSettingsService.IsSettingsOkAsync(meetingId);
            if (!platformSettings.Success)
                return BadRequest($"{platformSettings.Platform} Settings Have Problem.");

            var canJoinOnMeeting = await _handleMeetingService.CanJoinOnMeetingHandler(meetingId, joinOnMeetingRequest);

            if (!canJoinOnMeeting.Data)
                return canJoinOnMeeting.Errors.Count() > 1
                    ? BadRequest(canJoinOnMeeting.Errors)
                    : BadRequest(canJoinOnMeeting.Error);

            var url = await _connectionService.JoiningOnMeeting(meetingId, joinOnMeetingRequest);

            if (!url.Success)
                return url.Exception is null ? BadRequest(url.OnFailedMessage) : BadRequest(url.Exception.Data.ToString());

            await _handleMeetingService.JoiningOnMeetingOnDatabase(joinOnMeetingRequest.UserId, meetingId);

            return Redirect(url.Result);
        }

        [HttpPost("[action]/{meetingId}", Name = nameof(EndMeeting))]
        public async ValueTask<IActionResult> EndMeeting([FromRoute] string meetingId, [FromBody] string password)
        {
            var platformSettings = await _meetingSettingsService.IsSettingsOkAsync(meetingId);
            if (!platformSettings.Success)
                return BadRequest($"{platformSettings.Platform} Settings Have Problem.");

            var endExistMeeting = await _connectionService.EndExistMeeting(meetingId, password);

            if (!endExistMeeting.Success)
                return endExistMeeting.Exception is null
                    ? BadRequest(endExistMeeting.OnFailedMessage)
                    : BadRequest(new { endExistMeeting.Exception.Message });

            var handleEnd = await _handleMeetingService.EndMeetingHandlerOnDatabase(meetingId, DateTime.Now);

            if (handleEnd.Data is null)
                return handleEnd.Errors.Count() > 1 ? BadRequest(handleEnd.Errors) : BadRequest(handleEnd.Error);

            return Ok(handleEnd.Data);
        }
    }
}
