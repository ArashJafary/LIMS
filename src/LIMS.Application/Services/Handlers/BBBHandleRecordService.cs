using BigBlueButtonAPI.Core;
using BigBlueButtonAPI.Common;
using LIMS.Application.Models;
using Microsoft.Extensions.Logging;
using LIMS.Application.Services.Interfaces;
using LIMS.Domain.Services;

namespace LIMS.Application.Services.Handlers
{
    public class HandleRecordService : IHandleRecordService
    {
        private readonly BigBlueButtonAPIClient _client;
        private readonly ILogger<HandleRecordService> _logger;

        string IHandleRecordService.RecordHandlerName => "bbb";

        public HandleRecordService(BigBlueButtonAPIClient client, ILogger<HandleRecordService> logger)
            => (_client, _logger) = (client, logger);

        public async ValueTask<ResultSingleResponse<List<Recording>>> GetAllRecordedVideos(string meetingId)
        {
            var result = new GetRecordingsRequest
            {
                meetingID = meetingId
            };

            var recordedVideos = await _client.GetRecordingsAsync(result);

            if (recordedVideos is null)
                return ResultSingleResponse<List<Recording>>.OnFailed("No Any Record Exists.");

            return ResultSingleResponse<List<Recording>>.OnSuccess(recordedVideos.recordings);
        }

        public async Task<ResultSingleResponse<PublishRecordingsResponse>> PublishRecordings(string recordId, bool publish)
        {
            try
            {
                var request = new PublishRecordingsRequest
                {
                    recordID = recordId,
                    publish = publish
                };

                var result = await _client.PublishRecordingsAsync(request);

                if (result.Returncode == Returncode.Failed)
                    return ResultSingleResponse<PublishRecordingsResponse>.OnFailed(result.Message);

                return ResultSingleResponse<PublishRecordingsResponse>.OnSuccess(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                return ResultSingleResponse<PublishRecordingsResponse>.OnFailed(exception.Message);
            }
        }
    }
}
