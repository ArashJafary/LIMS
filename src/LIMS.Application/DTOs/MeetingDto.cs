
using LIMS.Domain.Enumerables;

namespace LIMS.Application.DTOs
{
    public record MeetingAddDto(
        string MeetingId,
        bool IsRecord,
        bool IsBreakout,
        bool CanFreeJoinOnBreakout,
        string ParentId,
        string Name,
        string ModeratorPassword,
        string AttendeePassword,
        DateTime StartDateTime,
        DateTime EndDateTime,
        int LimitCapacity,
        ServerAddEditDto Server,
        bool AutoStartRecord,
        PlatformTypes Platform
        );

    public record MeetingEditDto(
    string MeetingId,
    bool IsRecord,
    string Name,
    string ModeratorPassword,
    string AttendeePassword,
    DateTime EndDateTime,
    int limitCapacity,
    bool CanFreeJoinOnBreakout
    );
}
