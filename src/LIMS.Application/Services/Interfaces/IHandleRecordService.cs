using BigBlueButtonAPI.Common;
using BigBlueButtonAPI.Core;
using LIMS.Application.Models;

namespace LIMS.Application.Services.Interfaces
{
    public interface IHandleRecordService
    {
        string RecordHandlerName { get; }

        ValueTask<ResultSingleResponse<List<Recording>>> GetAllRecordedVideos(string meetingId);
        Task<ResultSingleResponse<PublishRecordingsResponse>> PublishRecordings(string recordId, bool publish);
    }
}
