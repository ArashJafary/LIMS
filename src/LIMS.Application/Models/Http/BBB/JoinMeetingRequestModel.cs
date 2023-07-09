using BigBlueApi.Application.DTOs;
using BigBlueApi.Domain;

namespace LIMS.Application.Models.Http.BBB;
public record JoinMeetingRequestModel(string MeetingId, UserAddEditDto UserInformations, string MeetingPassword, string Token);