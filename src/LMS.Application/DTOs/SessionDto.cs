using BigBlueApi.Domain;

namespace BigBlueApi.Application.DTOs
{
    public record MeetingAddEditDto(
        string MeetingId,
        bool IsRecord,
        string Name,
        string ModeratorPassword,
        string AttendeePassword
        );
}
