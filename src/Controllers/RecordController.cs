using BigBlueButtonAPI.Core;
using Microsoft.AspNetCore.Mvc;

namespace BigBlueApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
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

    [HttpGet(Name =nameof(IsRecordings))]
    public async ValueTask<ActionResult> IsRecordings(string MeetingId)
    {
        //var setupOk = await IsBigBlueSettingsOkAsync();
        //if (!setupOk)
        //    return BadRequest();
        //var result = await _client.GetRecordingsAsync();
        //return Ok(result);

        var Request = new GetRecordingsRequest
        {
            meetingID = MeetingId
        };
        var Result= await _client.GetRecordingsAsync(Request);
        if(Result is null)
            return NotFound();

        return Ok(true);
        
            
        
    }

    [HttpPost(Name =nameof(PublishRecordings))]
    public async ValueTask<ActionResult> PublishRecordings(string recordID, bool publish)
    {
        var request = new PublishRecordingsRequest { 
            recordID = recordID,
            publish = publish 
        };
        var result = await _client.PublishRecordingsAsync(request);

        if (result.returncode == Returncode.FAILED)
            return BadRequest(result.message);
        return Ok(result);
    }
}
