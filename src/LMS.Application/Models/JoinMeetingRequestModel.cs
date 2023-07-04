using BigBlueApi.Domain;

namespace BigBlueApi.Models;
public record JoinMeetingRequestModel(string MeetingId,string FullName,string Alias,UserRoleTypes Role,string Password,string Token);