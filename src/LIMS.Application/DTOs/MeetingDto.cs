
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
        int LimitCapacity,
        ServerAddEditDto Server
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
