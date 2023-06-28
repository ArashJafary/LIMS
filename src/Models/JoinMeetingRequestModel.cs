using BigBlueApi.Domain;

namespace BigBlueApi.Models;
public record JoinMeetingRequestModel(string MeetingId,string FullName,string Alias,UserRoles Role,string Password,string Token);