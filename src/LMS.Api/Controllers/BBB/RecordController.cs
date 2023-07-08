using BigBlueButtonAPI.Core;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Services.Meeting.BBB;
using LIMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LIMS.Api.Controllers.BBB;

[ApiController]
[Route("api/BBB/[controller]")]
public class RecordController : ControllerBase
{
    private readonly BigBlueButtonAPIClient _client;
    private readonly BBBHandleMeetingService _handleMeetingService;
    private readonly BBBRecordServiceImpl _recordService;
    public RecordController(BigBlueButtonAPIClient client, BBBHandleMeetingService handleMeetingService,BBBRecordServiceImpl recordService) 
        => (_client, _handleMeetingService, _recordService) = (client,handleMeetingService,recordService);

    [NonAction]
    private async ValueTask<bool> IsBigBlueSettingsOkAsync(string meetingId)
    {
        var meeting = await _handleMeetingService.IsBigBlueButtonOk(meetingId);
        return meeting.Data;
    }

    [HttpGet("[action]", Name = nameof(Recordings))]
    public async ValueTask<ActionResult> Recordings(string meetingId)
    {
        var setupOk = await IsBigBlueSettingsOkAsync(meetingId);

        var result = new GetRecordingsRequest
        {
            meetingID = meetingId
        };
        var recordings = await _client.GetRecordingsAsync(result);
        if (recordings is null)
            return NotFound();

        return Ok(recordings);
    }

    [HttpPost("[action]", Name = nameof(PublishRecordings))]
    public async ValueTask<ActionResult> PublishRecordings(string recordId, bool publish)
    {
        var request = new PublishRecordingsRequest
        {
            recordID = recordId,
            publish = publish
        };
        var result = await _client.PublishRecordingsAsync(request);

        if (result.Returncode == Returncode.Failed)
            return BadRequest(result.Message);
        return Ok(result);
    }
}
