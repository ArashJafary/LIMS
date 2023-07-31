using LIMS.Application.DTOs;
using BigBlueButtonAPI.Core;
using LIMS.Application.Models.Http.BBB;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Services.Meeting.BBB;
using Microsoft.AspNetCore.Mvc;
using LIMS.Infrastructure.ExternalApi.BBB;

namespace LIMS.Api.Controllers.BBB
{

    [ApiController]
    [Route("api/BBB/Meeting")]
    public class MeetController : ControllerBase
    {
        #region Main Services
        private readonly BbbUserServiceImpl _userService;
        private readonly BbbConnectionService _connectionService;
        private readonly BbbHandleMeetingService _handleMeetingService;

        public MeetController(
            BbbUserServiceImpl userService,
            BbbConnectionService connectionService,
            BbbHandleMeetingService handleMeetingService
        ) =>
        (
            _handleMeetingService,
            _userService,
            _connectionService
        ) = (
            handleMeetingService,
            userService,
            connectionService
        );
        #endregion

        #region Non and Simple Methods
        [NonAction]
        private async ValueTask<bool> IsBigBlueSettingsOkAsync(string meetingId)
        {
            /* Check Settings Will Be Ok */
            var meeting = await _handleMeetingService.IsBigBlueButtonOk(meetingId);
            return meeting.Data;
        }
        #endregion

        #region Endpoints
        [HttpGet("[action]", Name = nameof(GetMeetingInformation))]
        public async ValueTask<IActionResult> GetMeetingInformation([FromQuery] GetMeetingInfoRequest meetingRequest)
        {
            /* Test BBB Is Ok */
            if (!await IsBigBlueSettingsOkAsync(meetingRequest.meetingID))
                return BadRequest("BigBlueButton Settings Have Problem.");

            /* Get Informations of Meeting For Indicate To Client */
            var getMeetingInformation = await _connectionService.GetMeetingInformations(meetingRequest.meetingID);
            if (!getMeetingInformation.Success)
                return BadRequest(getMeetingInformation.OnFailedMessage);

            /* Return Information To Client */
            return Ok(getMeetingInformation.Result);
        }

        [HttpPost("[action]", Name = nameof(CreateMeeting))]
        public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingRequestModel request)
        {
            /* Test BBB Is Ok */
            if (!await IsBigBlueSettingsOkAsync(request.MeetingId))
                return BadRequest("BigBlueButton Settings Have Problem.");

            /* Use Capable Server For Creating Meeting On That */
            var server = await _handleMeetingService.UseMostCapableAndActiveServer();
            if (server.Data is null)
                return BadRequest(server.Error);

            /* Change BBB Configuration Settings For New Server */
            var changeSettings = await _connectionService.ChangeServerSettings(server.Data);

            if (!changeSettings.Success)
                return BadRequest(changeSettings.OnFailedMessage);

            /* Create Meeting On BBB With its Apis */
            var createMeetingConnection = await _connectionService
                .CreateMeetingOnBigBlueButton(
                    new MeetingAddDto(
                        request.MeetingId,
                        request.IsRecord,
                        request.IsBreakout,
                        request.CanFreeJoinOnBreakout,
                        request.ParentId,
                        request.Name,
                        request.ModeratorPassword,
                        request.AttendeePassword,
                        request.StartDateTime,
                        request.EndDateTime,
                        request.LimitCapacity,
                        server.Data,
                        request.AutoStartRecord,
                        request.Platform
                    ));

            if (!createMeetingConnection.Success)
                return BadRequest(createMeetingConnection.OnFailedMessage);

            /* Create Meeting Information On Database */
            var createMeeting = await _handleMeetingService.CreateMeetingOnDatabase(createMeetingConnection.Result);

            if (createMeeting.Data is null)
                return BadRequest(createMeeting.Error);

            /* Get Information of Meeting For Indicate To Client */
            var getMeetingInformation = await _connectionService.GetMeetingInformations(request.MeetingId);

            if (!getMeetingInformation.Success)
                return BadRequest(getMeetingInformation.OnFailedMessage);

            /* Return Information To Client */
            return Ok(getMeetingInformation);
        }

        [HttpPost("[action]/{meetingId}", Name = nameof(JoinOnMeeting))]
        public async ValueTask<IActionResult> JoinOnMeeting(
            [FromRoute] string meetingId,
            [FromBody] JoinMeetingRequestModel request
        )
        {
            /* Test BBB Is Ok */
            if (!await IsBigBlueSettingsOkAsync(meetingId))
                return BadRequest("BigBlueButton Settings Have Problem.");

            /* Checking User Can Join On Meeting */
            var canJoinOnMeeting = await _handleMeetingService.CanJoinOnMeetingHandler(meetingId, request);

            if (!canJoinOnMeeting.Data)
                return canJoinOnMeeting.Errors.Count() > 1
                    ? BadRequest(canJoinOnMeeting.Errors)
                    : BadRequest(canJoinOnMeeting.Error);

            /* Join On Meeting With BBB Api */
            var url = await _connectionService.JoiningOnMeeting(meetingId, request);

            if (!url.Success)
                return url.Exception is null ? BadRequest(url.OnFailedMessage) : BadRequest(url.Exception.Data.ToString());

            /* Join Creating For Database */
            await _handleMeetingService.JoiningOnMeetingOnDatabase(request.UserId, meetingId);

            /* Redirect Into URL of Meeting */
            return Redirect(url.Result);
        }

        [HttpGet("[action]", Name = nameof(EndMeeting))]
        public async ValueTask<IActionResult> EndMeeting(string meetingId, string password)
        {
            /* Test BBB Is Ok */
            if (!await IsBigBlueSettingsOkAsync(meetingId))
                return BadRequest("BigBlueButton Settings Have Problem.");

            /* End Existed Meeting With BBB Api */
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
        #endregion
    }
}
