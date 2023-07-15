using BigBlueButtonAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigBlueButtonAPI.Common;
using LIMS.Application.Models;
using LIMS.Domain.Entities;

namespace LIMS.Application.Services.Record
{
    public class BBBHandleRecordService
    {
        private readonly BigBlueButtonAPIClient _client;

        public BBBHandleRecordService(BigBlueButtonAPIClient client)
            => _client = client;

        public async ValueTask<SingleResponse<List<Recording>>> GetAllRecordedVideosFromBBB(string meetingId)
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

        public async Task<SingleResponse<PublishRecordingsResponse>> PublishRecordings(string recordId,bool publish)
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
    }
}
