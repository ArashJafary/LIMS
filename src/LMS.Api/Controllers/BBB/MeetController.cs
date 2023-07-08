using System.Xml;
using BigBlueApi.Application.DTOs;
using BigBlueApi.Domain;
using BigBlueButtonAPI.Core;
using LIMS.Application.Models.Http.BBB;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Services.Meeting.BBB;
using LIMS.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LIMS.Api.Controllers.BBB;

[ApiController]
[Route("api/BBB/[controller]")]
public class MeetController : ControllerBase
{
    private readonly BBBHandleMeetingService _handleMeetingService;
    private readonly BBBMeetingServiceImpl _meetingService;
    private readonly BBBServerServiceImpl _serverService;
    private readonly BBBUserServiceImpl _userService;
    private readonly BBBMemberShipServiceImpl _memberShipService;
    private readonly BigBlueButtonAPIClient _client;

    public MeetController(
        BBBHandleMeetingService handleMeetingService,
        BBBMeetingServiceImpl meetingService,
        BBBServerServiceImpl serverService,
        BBBUserServiceImpl userService,
        BBBMemberShipServiceImpl memberShipService,
        BigBlueButtonAPIClient client
    ) =>
        (_handleMeetingService,_meetingService, _serverService, _userService, _memberShipService, _client) = (
            handleMeetingService,
            meetingService,
            serverService,
            userService,
            memberShipService,
            client
        );

    [NonAction]
    private async ValueTask<bool> IsBigBlueSettingsOkAsync(string meetingId)
    {
        var meeting = await _handleMeetingService.IsBigBlueButtonOk(meetingId);
        return meeting.Data;
    }

    [HttpGet("[action]", Name = nameof(GetMeetingInformation))]
    public async ValueTask<IActionResult> GetMeetingInformation([FromBody] string meetingId)
    {
        var result = await _client.GetMeetingInfoAsync(
            new GetMeetingInfoRequest
            { meetingID = meetingId }
        );

        return Ok(result);
    }

    [HttpPost("[action]", Name = nameof(CreateMeeting))]
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingRequestModel request)
    {
        var server = await _handleMeetingService.UseCapableServerCreateMeeting();
        if (server.Errors.Count() != 0)
            return server.Error == null || server.Error == string.Empty ? BadRequest(server.Errors) : BadRequest(server.Error);

        _client.UseServerSettings(new BigBlueButtonAPISettings
        {
            SharedSecret = server.Data.ServerSecret,
            ServerAPIUrl = server.Data.ServerUrl
        });

        var meetingCreateRequest = new CreateMeetingRequest
        {
            name = request.Name,
            meetingID = request.MeetingId,
            record = request.Record,
        };
        var result = await _client.CreateMeetingAsync(meetingCreateRequest);
        if (result.returncode == Returncode.FAILED)
            return BadRequest("A Problem Has Been Occurred in Creating Meet.");

        var createMeeting =await _handleMeetingService.HandleCreateMeeting(new MeetingAddEditDto(
            result.meetingID,
            request.Record,
            meetingCreateRequest.name,
            result.moderatorPW,
            result.attendeePW
        ));
        if (createMeeting.Data is null)
            return createMeeting.Errors.Count() > 1 
                ? BadRequest(createMeeting.Errors) 
                : BadRequest(createMeeting.Error);

        var meetingInfoRequest = new GetMeetingInfoRequest { meetingID = request.MeetingId };
        var resultInformation = await _client.GetMeetingInfoAsync(meetingInfoRequest);

        return Ok(resultInformation);
    }

    [HttpPost("[action]/{id}", Name = nameof(JoinOnMeeting))]
    public async ValueTask<IActionResult> JoinOnMeeting([FromQuery] long id, [FromBody] JoinMeetingRequestModel request)
    {

        var canJoinOnMeeting =await _handleMeetingService.CanJoinOnMeetingHandler(id, request);
        if (!canJoinOnMeeting.Data)
            return canJoinOnMeeting.Errors.Count() > 1
                ? BadRequest(canJoinOnMeeting.Errors)
                : BadRequest(canJoinOnMeeting.Error);

        var userId = await _userService.CreateUser(
            new UserAddEditDto(request.UserInformations.FullName, request.UserInformations.Alias, request.UserInformations.Role)
        );
        if (!userId.Success)
            if (userId.Exception is not null)
                return StatusCode(500);
            else
                return BadRequest(userId.OnFailedMessage);

        var requestJoin = new JoinMeetingRequest 
            { meetingID = request.MeetingId };

        if (request.UserInformations.Role == UserRoleTypes.Moderator)
        {
            requestJoin.password = request.MeetingPassword;
            requestJoin.userID = "1";
            requestJoin.fullName = request.UserInformations.FullName;
        }
        else if (request.UserInformations.Role == UserRoleTypes.Attendee)
        {
            requestJoin.password = request.MeetingPassword;
            requestJoin.userID = "2";
            requestJoin.fullName = request.UserInformations.FullName;
        }
        else
        {
            requestJoin.guest = true;
            requestJoin.fullName = request.UserInformations.FullName;
        }
        var url = _client.GetJoinMeetingUrl(requestJoin);

      

        return Redirect(url);
    }

    [HttpGet("[action]",Name = nameof(EndMeeting))]
    public async ValueTask<IActionResult> EndMeeting(string meetingId, string password)
    {
        var result = await _client.EndMeetingAsync(
            new EndMeetingRequest { meetingID = meetingId, password = password }
        );
        if (result.returncode == Returncode.FAILED)
            return BadRequest(result.message);

        var endMeeting = await _meetingService.StopRunning(meetingId);
        if (!endMeeting.Success)
            if (endMeeting.Exception is not null)
                return StatusCode(500);
            else
                return BadRequest(endMeeting.OnFailedMessage);
        else
            return Ok("Meeting is End.");
    }
}
