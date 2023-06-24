using BigBlueApi.Models;
using BigBlueButtonAPI.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BigBlueApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MeetController : ControllerBase
{
    private readonly BigBlueButtonAPIClient _client;

    public MeetController(BigBlueButtonAPIClient client) => _client = client;

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

    [HttpGet("[action]")]
    public async Task<IActionResult> GetMeetingInformations([FromQuery] string mettingId)
    {
        var result = await _client.IsMeetingRunningAsync(
            new IsMeetingRunningRequest { meetingID = Guid.NewGuid().ToString() }
        );
        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingRequestModel request)
    {
        var result = await _client.CreateMeetingAsync(
            new CreateMeetingRequest
            {
                name = request.Name,
                meetingID = request.MeetingId,
                record = true
            }
        );
        if (result.returncode == Returncode.FAILED)
            return BadRequest("A Problem Has Been Occurred in Creating Meet.");
        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
        xmlDoc.LoadXml(result.ToString());
        string jsonResult = JsonConvert.SerializeXmlNode(xmlDoc, Formatting.Indented, true);
        return Ok(jsonResult);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> JoinMeeting([FromBody] JoinMeetingRequestModel request)
    {
        var requestJoin = new JoinMeetingRequest { meetingID = request.MeetingId };
        if (request.Role == "1")
        {
            requestJoin.password = request.Password;
            requestJoin.userID = "10000";
            requestJoin.fullName = "Admin";
        }
        else
        {
            requestJoin.password = request.Password;
            requestJoin.userID = "20000";
            requestJoin.fullName = "User";
        }
        var url = _client.GetJoinMeetingUrl(requestJoin);
        return Redirect(url);
    }

    [HttpGet("[action]")]
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
