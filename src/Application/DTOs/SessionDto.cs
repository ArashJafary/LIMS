using BigBlueApi.Domain;

namespace BigBlueApi.Application.DTOs
{
    public record SessionEditDto(
        bool Recorded,
        string Name,
        string ModeratorPassword,
        string AttendeePassword
        );
}
