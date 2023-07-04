using BigBlueApi.Application.DTOs;
using LIMS.Domain.Entity;

namespace BigBlueApi.Application.Mappers
{
    public static class SessionMapper
    {
        public static Session Map(SessionAddEditDto sessionEditDto) =>
            new Session(
                 sessionEditDto.Recorded,
                 sessionEditDto.Name,
                 sessionEditDto.ModeratorPassword,
                 sessionEditDto.AttendeePassword
                );

        public static SessionAddEditDto Map(Session session) =>
            new SessionAddEditDto(
                session.MeetingId,
                session.Recorded,
                session.Name, 
                session.ModeratorPassword, 
                session.AttendeePassword
                );

    }
}
