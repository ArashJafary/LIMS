using BigBlueButtonAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigBlueButtonAPI.Common;
using LIMS.Application.Models;
using LIMS.Domain.Entities;
using LIMS.Application.Services.Database.BBB;
using Microsoft.Extensions.Logging;

namespace LIMS.Application.Services.Record
{
    public class BbbHandleRecordService
    {
        private readonly BigBlueButtonAPIClient _client;
        private readonly ILogger<BbbHandleRecordService> _logger;

        public BbbHandleRecordService(BigBlueButtonAPIClient client, ILogger<BbbHandleRecordService> logger)
            => (_client, _logger) = (client, logger);

        public async ValueTask<SingleResponse<List<Recording>>> GetAllRecordedVideosFromBbb(string meetingId)
        {
            try
            {
                var result = new GetRecordingsRequest
                {
                    meetingID = meetingId
                };

                var recordedVideos = await _client.GetRecordingsAsync(result);

                if (recordedVideos is null)
                    return SingleResponse<List<Recording>>.OnFailed("No Any Record Exists.");

                return SingleResponse<List<Recording>>.OnSuccess(recordedVideos.recordings);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                return SingleResponse<List<Recording>>.OnFailed(exception.Message);
            }
        }

        public async Task<SingleResponse<PublishRecordingsResponse>> PublishRecordings(string recordId, bool publish)
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
                    return SingleResponse<PublishRecordingsResponse>.OnFailed(result.Message);

                return SingleResponse<PublishRecordingsResponse>.OnSuccess(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                return SingleResponse<PublishRecordingsResponse>.OnFailed(exception.Message);
            }
        }
    }
}
