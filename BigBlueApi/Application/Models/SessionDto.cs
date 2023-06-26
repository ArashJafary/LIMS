using BigBlueApi.Domain;

namespace BigBlueApi.Application.Models
{
    public record SessionEditDto(
        bool Recorded,
        string Name,
        string ModeratorPassword,
        string AttendeePassword
        );
}
