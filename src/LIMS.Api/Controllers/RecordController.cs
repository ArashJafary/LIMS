using BigBlueButtonAPI.Core;
using LIMS.Application.Services.Database;
using LIMS.Application.Services.Handlers;
using LIMS.Application.Services.Interfaces;
using LIMS.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace LIMS.Api.Controllers
{
    [ApiController]
    [Route("api/BBB/[controller]")]
    public class RecordController : ControllerBase
    {
        private readonly BigBlueButtonAPIClient _client;
        private readonly IHandleMeetingService _handleMeetingService;
        private readonly RecordServiceImpl _recordService;
        private readonly IHandleRecordService _handleRecordService;
        public RecordController(
            BigBlueButtonAPIClient client,
            BbbHandleMeetingService handleMeetingService,
            RecordServiceImpl recordService,
            IHandleRecordService handleRecord)
            => (_client, _handleMeetingService, _recordService, _handleRecordService) = (client, handleMeetingService, recordService, handleRecord);

        [NonAction]
        private async ValueTask<bool> IsBigBlueSettingsOkAsync(string meetingId)
        {
            var meeting = await _handleMeetingService.IsOkSettings(meetingId);

            return meeting.Data;
        }

        [HttpGet("[action]", Name = nameof(GetAllRecordings))]
        public async ValueTask<ActionResult> GetAllRecordings(string meetingId)
        {
            if (!await IsBigBlueSettingsOkAsync(meetingId))
                return BadRequest("BigBlueButton Settings Have Problem.");

            var records = await _handleRecordService.GetAllRecordedVideos(meetingId);

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
