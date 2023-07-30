using BigBlueButtonAPI.Core;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Services.Meeting.BBB;
using LIMS.Application.Services.Record;
using Microsoft.AspNetCore.Mvc;

namespace LIMS.Api.Controllers.BBB
{
    [ApiController]
    [Route("api/BBB/[controller]")]
    public class RecordController : ControllerBase
    {
        private readonly BigBlueButtonAPIClient _client;
        private readonly BbbHandleMeetingService _handleMeetingService;
        private readonly BbbRecordServiceImpl _recordService;
        private readonly BbbHandleRecordService _handleRecordService;
        public RecordController(
            BigBlueButtonAPIClient client,
            BbbHandleMeetingService handleMeetingService,
            BbbRecordServiceImpl recordService,
            BbbHandleRecordService handleRecord)
            => (_client, _handleMeetingService, _recordService, _handleRecordService) = (client, handleMeetingService, recordService, handleRecord);

        [NonAction]
        private async ValueTask<bool> IsBigBlueSettingsOkAsync(string meetingId)
        {
            /* Check Settings Will Be Ok */
            var meeting = await _handleMeetingService.IsBigBlueButtonOk(meetingId);

            return meeting.Data;
        }

        [HttpGet("[action]", Name = nameof(GetAllRecordings))]
        public async ValueTask<ActionResult> GetAllRecordings(string meetingId)
        {
            if (!await IsBigBlueSettingsOkAsync(meetingId))
                return BadRequest("BigBlueButton Settings Have Problem.");

            var records = await _handleRecordService.GetAllRecordedVideosFromBbb(meetingId);

            if (records.Data is null)
                return records.Errors.Count() > 1
                    ? NotFound(records.Errors)
                    : NotFound(records.Error);

            return Ok(records.Data);
        }

        [HttpPost("[action]", Name = nameof(PublishRecordings))]
        public async ValueTask<ActionResult> PublishRecordings(string recordId, bool publish)
        {
            var record = await _handleRecordService.PublishRecordings(recordId, publish);

            if (record.Data is null)
                return record.Errors.Count() > 1 ? BadRequest(record.Errors) : BadRequest(record.Error);

            return Ok(record.Data);
        }
    }
}
