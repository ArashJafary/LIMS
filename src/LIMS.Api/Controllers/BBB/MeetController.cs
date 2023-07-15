using System.Xml;
using Azure.Core;
using LIMS.Application.DTOs;
using LIMS.Domain;
using BigBlueButtonAPI.Core;
using Hangfire;
using LIMS.Application.Models.Http.BBB;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Services.Meeting.BBB;
using LIMS.Application.Services.Schedulers.HangFire;
using LIMS.Domain;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using LIMS.Infrastructure.Services.Api.BBB;
using LIMS.Infrastructure.Services.Api.BBB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LIMS.Api.Controllers.BBB;

[ApiController]
[Route("api/BBB/[controller]")]
public class MeetController : ControllerBase
{
    #region Main Services
    private readonly BBBUserServiceImpl _userService;
    private readonly BBBConnectionService _connectionService;
    private readonly BBBHandleMeetingService _handleMeetingService;

    public MeetController(
        BBBUserServiceImpl userService,
        BBBConnectionService connectionService,
        BBBHandleMeetingService handleMeetingService
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
    public async ValueTask<IActionResult> GetMeetingInformation([FromBody] string meetingId)
    {
        /* Test BBB Is Ok */
        var meeting = await _handleMeetingService.IsBigBlueButtonOk(meetingId);
        if (meeting.Data)
            return BadRequest("BigBlueButton Settings Have Problem.");


        /* Get Informations of Meeting For Indicate To Client */
        var getMeetingInformation = await _connectionService.GetMeetingInformations(meetingId);
        if (!getMeetingInformation.Success)
            return getMeetingInformation.Exception is null
                ? BadRequest(getMeetingInformation.OnFailedMessage)
                : BadRequest(getMeetingInformation.Exception.Data.ToString());


        /* Return Information To Client */
        return Ok(getMeetingInformation.Result);
    }

    [HttpPost("[action]", Name = nameof(CreateMeeting))]
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingRequestModel request)
    {
        /* Test BBB Is Ok */
        var meeting = await _handleMeetingService.IsBigBlueButtonOk(request.MeetingId);
        if (meeting.Data)
            return BadRequest("BigBlueButton Settings Have Problem.");


        /* Use Capable Server For Creating Meeting On That */
        var server = await _handleMeetingService.UseCapableServerCreateMeeting();
        if (server.Errors.Count() != 0)
            return server.Error == null || server.Error == string.Empty
                ? BadRequest(server.Errors)
                : BadRequest(server.Error);


        /* Change BBB Configuration Settings For New Server */
        var changeSettings = await _connectionService.ChangeServerSettings(
            new BigBlueButtonAPISettings
            {
                ServerAPIUrl = server.Data.ServerUrl,
                SharedSecret = server.Data.ServerSecret
            },
            server.Data);
        if (!changeSettings.Success)
            return changeSettings.Exception is null
                ? BadRequest(changeSettings.OnFailedMessage)
                : BadRequest(changeSettings.Exception.Data.ToString());


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
                )
                    );

        if (!createMeetingConnection.Success)
            return createMeetingConnection.Exception is null
                ? BadRequest(createMeetingConnection.OnFailedMessage)
                : BadRequest(createMeetingConnection.Exception.Data.ToString());


        /* Create Meeting Information On Database */
        var createMeeting = await _handleMeetingService.CreateMeetingOnDatabase(createMeetingConnection.Result);
        if (createMeeting.Data is null)
            return createMeeting.Errors.Count() > 1
                ? BadRequest(createMeeting.Errors)
                : BadRequest(createMeeting.Error);


        /* Get Information of Meeting For Indicate To Client */
        var getMeetingInformation = await _connectionService.GetMeetingInformations(request.MeetingId);
        if (!getMeetingInformation.Success)
            return getMeetingInformation.Exception is null
                ? BadRequest(getMeetingInformation.OnFailedMessage)
                : BadRequest(getMeetingInformation.Exception.Data.ToString());


        /* Return Information To Client */
        return Ok(getMeetingInformation);
    }

    [HttpPost("[action]/{id}", Name = nameof(JoinOnMeeting))]
    public async ValueTask<IActionResult> JoinOnMeeting(
        [FromQuery] long id,
        [FromBody] JoinMeetingRequestModel request
    )
    {
        /* Test BBB Is Ok */
        var meeting = await _handleMeetingService.IsBigBlueButtonOk(request.MeetingId);
        if (meeting.Data)
            return BadRequest("BigBlueButton Settings Have Problem.");


        /* Checking User Can Join On Meeting */
        var canJoinOnMeeting = await _handleMeetingService.CanJoinOnMeetingHandler(id, request);
        if (!canJoinOnMeeting.Data)
            return canJoinOnMeeting.Errors.Count() > 1
                ? BadRequest(canJoinOnMeeting.Errors)
                : BadRequest(canJoinOnMeeting.Error);


        /* Join On Meeting With BBB Api */
        var url = await _connectionService.JoiningOnMeeting(request);
        if (!url.Success)
            return url.Exception is null ? BadRequest(url.OnFailedMessage) : BadRequest(url.Exception.Data.ToString());


        /* Join Creating For Database */
        await _handleMeetingService.JoiningOnMeetingOnDatabase(request.UserId, request.MeetingId);


        /* Redirect Into URL of Meeting */
        return Redirect(url.Result);
    }

    [HttpGet("[action]", Name = nameof(EndMeeting))]
    public async ValueTask<IActionResult> EndMeeting(string meetingId, string password)
    {
        /* Test BBB Is Ok */
        var meeting = await _handleMeetingService.IsBigBlueButtonOk(meetingId);
        if (meeting.Data)
            return BadRequest("BigBlueButton Settings Have Problem.");


        /* End Existed Meeting With BBB Api */
        var endExistMeeting = await _connectionService.EndExistMeeting(meetingId, password);
        if (!endExistMeeting.Success)
            return endExistMeeting.Exception is null
                ? BadRequest(endExistMeeting.OnFailedMessage)
                : BadRequest(endExistMeeting.Exception.Data.ToString());

        /* End Meeting On Database */
        var handleEnd = await _handleMeetingService.EndMeetingHandlerOnDatabase(meetingId);
        if (handleEnd.Data is null)
            return handleEnd.Errors.Count() > 1 ? BadRequest(handleEnd.Errors) : BadRequest(handleEnd.Error);

        /* Return Datas Into Client */
        return Ok(handleEnd.Data);
    }

    #endregion
}
