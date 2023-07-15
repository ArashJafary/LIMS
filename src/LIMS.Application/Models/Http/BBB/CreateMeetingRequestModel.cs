using LIMS.Application.DTOs;
using LIMS.Domain.Enumerables;

namespace LIMS.Application.Models.Http.BBB;
public record CreateMeetingRequestModel(
    string Name,
    bool IsPrivate,
    string MeetingId, 
    bool IsRecord,
    bool AutoStartRecord,
    string ModeratorPassword,
    string AttendeePassword,
    DateTime StartDateTime,
    DateTime EndDateTime,
    int LimitCapacity,
    bool IsBreakout,
    bool CanFreeJoinOnBreakout,
    string ParentId,
    List<long> UsersAccessed,
    PlatformTypes Platform
    );