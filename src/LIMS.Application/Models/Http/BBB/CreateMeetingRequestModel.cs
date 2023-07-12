using LIMS.Application.DTOs;

namespace LIMS.Application.Models.Http.BBB;
public record CreateMeetingRequestModel(
    string Name,
    bool IsPrivate,
    string MeetingId, 
    bool MustRecord,
    string ModeratorPassword,
    string AttendeePassword,
    DateTime StartDateTime,
    int LimitCapacity,
    bool IsBreakout,
    bool CanFreeJoinOnBreakout,
    string ParentId,
    List<long> UsersAccessed
    );