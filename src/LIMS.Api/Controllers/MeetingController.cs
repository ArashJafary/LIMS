using LIMS.Application.DTOs;
using BigBlueButtonAPI.Core;
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

        [HttpGet("[action]", Name = nameof(GetMeetingInformation))]
        public async ValueTask<IActionResult> GetMeetingInformation([FromQuery] GetMeetingInfoRequest meetingRequest)
        {
            /* Test Platform Is Ok */
            var platformSettings = await _meetingSettingsService.IsSettingsOkAsync(meetingRequest.meetingID);
            if (!platformSettings.Success)
                return BadRequest($"{platformSettings.Platform} Settings Have Problem.");

            /* Get Informations of Meeting For Indicate To Client */
            var getMeetingInformation = await _connectionService.GetMeetingInformations(meetingRequest.meetingID);
            if (!getMeetingInformation.Success)
                return BadRequest(getMeetingInformation.OnFailedMessage);

            /* Return Information To Client */
            return Ok(getMeetingInformation.Result);
        }

        [HttpPost("[action]", Name = nameof(CreateMeeting))]
        public async Task<IActionResult> CreateMeeting([FromBody] BbbCreateMeetingRequestModel createMeetingRequest)
        {
            /* Use Capable Server For Creating Meeting On That */
            var server = await _handleMeetingService.UseMostCapableAndActiveServer();
            if (server.Data is null)
                return BadRequest(server.Error);

            /* Change Server Configuration Settings For New Server */
            var changeSettings = await _connectionService.ChangeServerSettings(server.Data);

            if (!changeSettings.Success)
                return BadRequest(changeSettings.OnFailedMessage);

            /* Create Meeting On Platform With its Apis */
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
                        server.Data,
                        createMeetingRequest.AutoStartRecord,
                        createMeetingRequest.Platform
                    ));

            if (!createMeetingConnection.Success)
                return BadRequest(createMeetingConnection.OnFailedMessage);

            /* Create Meeting Information On Database */
            var createMeeting = await _handleMeetingService.CreateMeetingOnDatabase(createMeetingConnection.Result);

            if (createMeeting.Data is null)
                return BadRequest(createMeeting.Error);

            /* Get Information of Meeting For Indicate To Client */
            var getMeetingInformation = await _connectionService.GetMeetingInformations(createMeetingRequest.MeetingId);

            if (!getMeetingInformation.Success)
                return BadRequest(getMeetingInformation.OnFailedMessage);

            /* Return Information To Client */
            return Ok(getMeetingInformation);
        }

        [HttpPost("[action]/{meetingId}", Name = nameof(JoinOnMeeting))]
        public async ValueTask<IActionResult> JoinOnMeeting(
            [FromRoute] string meetingId,
            [FromBody] BbbJoinMeetingRequestModel joinOnMeetingRequest
        )
        {
            /* Test Platform Is Ok */
            var platformSettings = await _meetingSettingsService.IsSettingsOkAsync(meetingId);
            if (!platformSettings.Success)
                return BadRequest($"{platformSettings.Platform} Settings Have Problem.");

            /* Checking User Can Join On Meeting */
            var canJoinOnMeeting = await _handleMeetingService.CanJoinOnMeetingHandler(meetingId, joinOnMeetingRequest);

            if (!canJoinOnMeeting.Data)
                return canJoinOnMeeting.Errors.Count() > 1
                    ? BadRequest(canJoinOnMeeting.Errors)
                    : BadRequest(canJoinOnMeeting.Error);

            /* Join On Meeting With Plaform Api */
            var url = await _connectionService.JoiningOnMeeting(meetingId, joinOnMeetingRequest);

            if (!url.Success)
                return url.Exception is null ? BadRequest(url.OnFailedMessage) : BadRequest(url.Exception.Data.ToString());

            /* Join Creating For Database */
            await _handleMeetingService.JoiningOnMeetingOnDatabase(joinOnMeetingRequest.UserId, meetingId);

            /* Redirect Into URL of Meeting */
            return Redirect(url.Result);
        }

        [HttpGet("[action]", Name = nameof(EndMeeting))]
        public async ValueTask<IActionResult> EndMeeting(string meetingId, string password)
        {
            /* Test Platform Is Ok */
            var platformSettings = await _meetingSettingsService.IsSettingsOkAsync(meetingId);
            if (!platformSettings.Success)
                return BadRequest($"{platformSettings.Platform} Settings Have Problem.");

            /* End Existed Meeting With Platform Api */
            var endExistMeeting = await _connectionService.EndExistMeeting(meetingId, password);

            if (!endExistMeeting.Success)
                return endExistMeeting.Exception is null
                    ? BadRequest(endExistMeeting.OnFailedMessage)
                    : BadRequest(new { endExistMeeting.Exception.Message });

            /* End Meeting On Database */
            var handleEnd = await _handleMeetingService.EndMeetingHandlerOnDatabase(meetingId, DateTime.Now);

            if (handleEnd.Data is null)
                return handleEnd.Errors.Count() > 1 ? BadRequest(handleEnd.Errors) : BadRequest(handleEnd.Error);

            /* Return Datas Into Client */
            return Ok(handleEnd.Data);
        }
    }
}
