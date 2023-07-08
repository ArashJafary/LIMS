using BigBlueApi.Domain;

namespace BigBlueApi.Application.DTOs
{
    public record MeetingAddDto(
        string MeetingId,
        bool IsRecord,
        string Name,
        string ModeratorPassword,
        string AttendeePassword
        );

    public record MeetingEditDto(
    string MeetingId,
    bool IsRecord,
    string Name,
    string ModeratorPassword,
    string AttendeePassword,
    DateTime EndDateTime,
    int limitCapacity
    );
}
