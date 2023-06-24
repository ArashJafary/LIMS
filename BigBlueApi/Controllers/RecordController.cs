using BigBlueButtonAPI.Core;
using Microsoft.AspNetCore.Mvc;

namespace BigBlueApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RecordController : ControllerBase
{
    private readonly BigBlueButtonAPIClient _client;

    public RecordController(BigBlueButtonAPIClient client) => _client = client;

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
    public async Task<ActionResult> Recordings()
    {
        var setupOk = await IsBigBlueSettingsOkAsync();
        if (!setupOk)
            return BadRequest();
        var result = await _client.GetRecordingsAsync();
        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> PublishRecordings(string recordID, string type)
    {
        var request = new PublishRecordingsRequest { recordID = recordID, publish = type == "1" };
        var result = await _client.PublishRecordingsAsync(request);

        if (result.returncode == Returncode.FAILED)
            return BadRequest(result.message);
        return Ok(result);
    }
}
