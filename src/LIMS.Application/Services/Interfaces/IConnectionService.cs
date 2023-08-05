using BigBlueButtonAPI.Core;
using LIMS.Application.DTOs;
using LIMS.Application.Models;
using LIMS.Application.Models.Http;

namespace LIMS.Application.Services.Interfaces
{
    public interface IConnectionService
    {
        ValueTask<OperationResult<MeetingAddDto>> CreateMeetingOnPlatform(MeetingAddDto meetingRequestModel);
        Task<OperationResult> ChangeServerSettings(ServerAddEditDto server);
        Task<OperationResult<string>> JoiningOnMeeting(string meetingId, BbbJoinMeetingRequestModel joinOnMeetingRequest);
        Task<OperationResult> EndExistMeeting(string meetingId, string moderatorPassword);
        ValueTask<OperationResult<GetMeetingInfoResponse>> GetMeetingInformations(string meetingId);
    }
}
