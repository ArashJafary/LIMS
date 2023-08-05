using LIMS.Application.DTOs;
using LIMS.Domain;

namespace LIMS.Application.Models.Http;
public record BbbJoinMeetingRequestModel(long UserId, string MeetingPassword);

