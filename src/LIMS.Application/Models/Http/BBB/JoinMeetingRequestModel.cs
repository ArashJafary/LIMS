using LIMS.Application.DTOs;
using LIMS.Domain;

namespace LIMS.Application.Models.Http.BBB;
public record JoinMeetingRequestModel(string MeetingId,long UserId, string MeetingPassword, string Token);