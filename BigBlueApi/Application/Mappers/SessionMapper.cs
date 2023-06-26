using BigBlueApi.Application.Models;
using BigBlueApi.Domain;

namespace BigBlueApi.Application.Mappers
{
    public static class SessionMapper
    {
        public static Session Map(SessionEditDto sessionEditDto) =>
            new Session(
                 sessionEditDto.Recorded,
                 sessionEditDto.Name,
                 sessionEditDto.ModeratorPassword,
                 sessionEditDto.AttendeePassword
                );

        public static SessionEditDto Map(Session session) =>
            new SessionEditDto(
                session.Recorded,
                session.Name, 
                session.ModeratorPassword, 
                session.AttendeePassword
                );

    }
}
