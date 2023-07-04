using BigBlueApi.Application.DTOs;
using LIMS.Domain.Entity;

namespace BigBlueApi.Application.Mappers
{
    public static class SessionMapper
    {
        public static Meeting Map(SessionAddEditDto sessionEditDto) =>
            new Meeting(
                 sessionEditDto.Recorded,
                 sessionEditDto.Name,
                 sessionEditDto.ModeratorPassword,
                 sessionEditDto.AttendeePassword
                );

        public static SessionAddEditDto Map(Meeting session) =>
            new SessionAddEditDto(
                session.MeetingId,
                session.Recorded,
                session.Name, 
                session.ModeratorPassword, 
                session.AttendeePassword
                );

    }
}
