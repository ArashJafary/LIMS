namespace LIMS.Application.Models.Http.BBB;
public record CreateMeetingRequestModel(string Name, string MeetingId, bool MustRecord,string ModeratorPassword,string AttendeePassword);