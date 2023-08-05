using LIMS.Application.DTOs;
using LIMS.Application.Models;
using LIMS.Application.Models.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Domain.Services
{
    public interface IHandleMeetingService
    {

        string MeetingHandlerName { get; }

        Task<ResultSingleResponse<ServerAddEditDto>> UseMostCapableAndActiveServer();
        ValueTask<ResultSingleResponse<string>> CreateMeetingOnDatabase(MeetingAddDto meeting);
        ValueTask<ResultSingleResponse<bool>> IsOkSettings(string meetingId);
        ValueTask<ResultSingleResponse<bool>> CanJoinOnMeetingHandler(string meetingId, BbbJoinMeetingRequestModel joinRequest);
        ValueTask<ResultSingleResponse<long>> JoiningOnMeetingOnDatabase(long userId, string meetingId);
        Task<ResultSingleResponse<string>> EndMeetingHandlerOnDatabase(string meetingId, DateTime now);
    }
}
