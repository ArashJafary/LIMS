using LIMS.Application.DTOs;
using LIMS.Domain;

namespace LIMS.Application.Models.Http.BBB;
public record JoinMeetingRequestModel(long UserId, string MeetingPassword);

