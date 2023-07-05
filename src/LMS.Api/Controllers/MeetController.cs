using System.Xml;
using BigBlueApi.Application.DTOs;
using BigBlueApi.Domain;
using BigBlueApi.Models;
using BigBlueApi.Persistence;
using BigBlueButtonAPI.Core;
using LIMS.Application.Services.Database.BBB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BigBlueApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MeetController : ControllerBase
{
    private readonly SessionServiceImp _sessionService;
    private readonly BBBServerServiceImpl _serverService;
    private readonly BBBUserServiceImpl _userService;
    private readonly MemberShipServiceImp _memberShipService;
    private readonly BigBlueButtonAPIClient _client;

    public MeetController(
        SessionServiceImp sessionService,
        BBBServerServiceImpl serverService,
        BBBUserServiceImpl userService,
        MemberShipServiceImp memberShipService,
        BigBlueButtonAPIClient client
    ) =>
        (_sessionService, _serverService, _userService, _memberShipService, _client) = (
            sessionService,
            serverService,
            userService,
            memberShipService,
            client
        );

    [NonAction]
    private async Task<bool> IsBigBlueSettingsOkAsync()
    {
        try
        {
            var result = await _client.IsMeetingRunningAsync(
                new IsMeetingRunningRequest { meetingID = Guid.NewGuid().ToString() }
            );
            if (result.returncode == Returncode.FAILED)
                return false;
            return true;
        }
        catch
        {
            return false;
        }
    }

    [HttpGet("[action]", Name = nameof(GetMeetingInformations))]
    public async Task<IActionResult> GetMeetingInformations([FromQuery] string meetingId)
    {
        var result = await _client.GetMeetingInfoAsync(
            new GetMeetingInfoRequest { meetingID = meetingId }
        );
        return Ok(result);
    }

    [HttpPost("[action]", Name = nameof(CreateMeeting))]
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingRequestModel request)
    {
        var server = await _serverService.MostCapableServer();

        var meetingCreateRequest = new CreateMeetingRequest
        {
            name = request.Name,
            meetingID = request.MeetingId,
            record = request.Record,
        };
        var result = await _client.CreateMeetingAsync(meetingCreateRequest);

        if (result.returncode == Returncode.FAILED)
            return BadRequest("A Problem Has Been Occurred in Creating Meet.");

        await _sessionService.CreateSession(
            new SessionAddEditDto(
                result.meetingID,
                request.Record,
                meetingCreateRequest.name,
                result.moderatorPW,
                result.attendeePW
            )
        );

        var meetingInfoRequest = new GetMeetingInfoRequest { meetingID = request.MeetingId };

        var resultInfo = await _client.GetMeetingInfoAsync(meetingInfoRequest);

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(result.ToString()!);
        string jsonResult = JsonConvert.SerializeXmlNode(
            xmlDoc,
            Newtonsoft.Json.Formatting.Indented,
            true
        );

        return Ok(jsonResult);
    }

    [HttpPost("[action]", Name = nameof(JoinMeeting))]
    public async ValueTask<IActionResult> JoinMeeting([FromBody] JoinMeetingRequestModel request)
    {
        var server = await _sessionService.Find(request.MeetingId);

        if (await _memberShipService.CanJoinUserOnSession(request.MeetingId))
            return BadRequest("Joining into This Class Not Accessed.");

        if (await _serverService.CanJoinServer(server.Id))
            return BadRequest("Server is Fulled!");

        if (!await _sessionService.CanLogin(request.MeetingId, request.Role, request.Password))
            return BadRequest("Your Credentials are Not Valid.");

        var userId = await _userService.CreateUser(
            new UserAddEditDto(request.FullName, request.Alias, request.Role)
        );

        var requestJoin = new JoinMeetingRequest { meetingID = request.MeetingId };
        if (request.Role == UserRoles.Moderator)
        {
            requestJoin.password = request.Password;
            requestJoin.userID = "1";
            requestJoin.fullName = request.FullName;
        }
        else if (request.Role == UserRoles.Attendee)
        {
            requestJoin.password = request.Password;
            requestJoin.userID = "2";
            requestJoin.fullName = request.FullName;
        }
        else
        {
            requestJoin.guest = true;
            requestJoin.fullName = request.FullName;
        }

        try
        {
            var url = _client.GetJoinMeetingUrl(requestJoin);
            await _memberShipService.JoinUser(request.MeetingId, userId);

            return Redirect(url);
        }
        catch (System.Exception exception)
        {
            return BadRequest(new { Message = exception.Message, Data = exception.Data });
        }
    }

    [HttpGet]
    public async Task<IActionResult> EndMeeting(string meetingId, string password)
    {
        var result = await _client.EndMeetingAsync(
            new EndMeetingRequest { meetingID = meetingId, password = password }
        );
        if (result.returncode == Returncode.FAILED)
            return BadRequest(result.message);

        await _sessionService.StopRunning(meetingId);
        return Ok("Meeting is End.");
    }
}
