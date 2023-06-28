using BigBlueApi.Application.DTOs;
using BigBlueApi.Application.Services;
using BigBlueApi.Domain;
using BigBlueApi.Models;
using BigBlueApi.Persistence;
using BigBlueApi.Persistence.Repository;
using BigBlueButtonAPI.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BigBlueApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MeetController : ControllerBase
{
    private readonly SessionServiceImp _sessionService;
    private readonly ServerServiceImp _serverService;
    private readonly UserServiceImp _userService;
    private readonly MemberShipServiceImp _memberShipService;
    private readonly BigBlueButtonAPIClient _client;

    public MeetController(
        SessionServiceImp sessionService,
        ServerServiceImp serverService,
        UserServiceImp userService,
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

    [HttpGet]
    public async Task<IActionResult> GetMeetingInformations([FromQuery] string meetingId)
    {
        var result = await _client.GetMeetingInfoAsync(
            new GetMeetingInfoRequest { meetingID = meetingId }
        );
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingRequestModel request)
    {
        var meetingCreateRequest = new CreateMeetingRequest
        {
            name = request.Name,
            meetingID = request.MeetingId,
            record = true,
        };
        var result = await _client.CreateMeetingAsync(meetingCreateRequest);
        if (result.returncode == Returncode.FAILED)
            return BadRequest("A Problem Has Been Occurred in Creating Meet.");

        var meetingInfoRequest = new GetMeetingInfoRequest { meetingID = request.MeetingId };
        var resultInfo = await _client.GetMeetingInfoAsync(meetingInfoRequest);

        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
        xmlDoc.LoadXml(result.ToString()!);
        string jsonResult = JsonConvert.SerializeXmlNode(xmlDoc, Formatting.Indented, true);

        return Ok(jsonResult);
    }

    [HttpPost]
    public async ValueTask<IActionResult> JoinMeeting([FromBody] JoinMeetingRequestModel request)
    {
        if (await _memberShipService.CanJoinUserOnSession(request.MeetingId))
            return BadRequest("Joining into This Class Not Accessed.");
        var server = await _sessionService.Find(request.MeetingId);
        if (await _serverService.CanJoinServer(server.Id))
            return BadRequest("Server is Fulled!");

        await _userService.CreateUser(new UserAddEditDto(request.FullName,request.Alias,request.Role));
        var requestJoin = new JoinMeetingRequest { meetingID = request.MeetingId };
        if (request.Role == UserRoles.Moderator)
        {
            requestJoin.password = request.Password;
            requestJoin.userID = "10000";
            requestJoin.fullName = request.FullName;
        }
        else
        {
            requestJoin.password = request.Password;
            requestJoin.userID = "20000";
            requestJoin.fullName = "Attendee";
        }
                    return Redirect("");

        try
        {
            var url = _client.GetJoinMeetingUrl(requestJoin);
            // _memberShipService.JoinUser(request.MeetingId,)
            return Redirect(url);
        }
        catch (System.Exception exception) { }
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
