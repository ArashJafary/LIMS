using BigBlueApi.Application.DTOs;
using LIMS.Domain.Entity;

namespace BigBlueApi.Application.Mappers
{
    public static class MeetingDtoMapper
    {
        public static Meeting Map(MeetingAddEditDto meetingDto) =>
            new Meeting(
                 meetingDto.Recorded,
                 meetingDto.Name,
                 meetingDto.ModeratorPassword,
                 meetingDto.AttendeePassword
                );

        public static MeetingAddEditDto Map(Meeting record) =>
            new MeetingAddEditDto(
                record.MeetingId,
                record.IsRecord,
                record.Name, 
                record.ModeratorPassword, 
                record.AttendeePassword
                );

    }
}
