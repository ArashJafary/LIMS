using BigBlueApi.Domain;

namespace BigBlueApi.Application.DTOs
{
    public record MeetingAddEditDto(
        string MeetingId,
        bool Recorded,
        string Name,
        string ModeratorPassword,
        string AttendeePassword
        );
}
