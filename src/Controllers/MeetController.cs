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

    public MeetController(SessionServiceImp sessionService, ServerServiceImp serverService , UserServiceImp userService , MemberShipServiceImp memberShipService, BigBlueButtonAPIClient client) =>
        (_sessionService,_serverService,_userService,_memberShipService,_client) = (sessionService, serverService, userService, memberShipService, client);

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
        var result = await _client.GetMeetingInfoAsync(new GetMeetingInfoRequest { meetingID = meetingId });
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
    public async Task<IActionResult> JoinMeeting([FromBody] JoinMeetingRequestModel request)
    {
        var requestJoin = new JoinMeetingRequest { meetingID = request.MeetingId };
        if (request.Role == "1")
        {
            requestJoin.password = request.Password;
            requestJoin.userID = "10000";
            requestJoin.fullName = "Moderator";
        }
        else
        {
            requestJoin.password = request.Password;
            requestJoin.userID = "20000";
            requestJoin.fullName = "Attendee";
        }
        var url = _client.GetJoinMeetingUrl(requestJoin);
        return Redirect(url);
    }

    [HttpGet]
    public async Task<IActionResult> EndMeeting(string meetingId, string password)
    {
        var result = await _client.EndMeetingAsync(
            new EndMeetingRequest { meetingID = meetingId, password = password }
        );
        if (result.returncode == Returncode.FAILED)
            return BadRequest(result.message);
        return Ok("Meeting is End.");
    }
}
