using BigBlueApi.Application.DTOs;
using LIMS.Domain.Entity;

namespace BigBlueApi.Application.Mappers
{
    public static class MeetingDtoMapper
    {
        public static Meeting Map(MeetingAddEditDto meetingDto) =>
            new Meeting(
                 meetingDto.MeetingId,
                 meetingDto.IsRecord,
                 meetingDto.Name,
                 meetingDto.ModeratorPassword,
                 meetingDto.AttendeePassword
                );

        public static MeetingAddEditDto Map(Meeting meeting) =>
            new MeetingAddEditDto(
                meeting.MeetingId,
                meeting.IsRecord,
                meeting.Name,
                meeting.ModeratorPassword,
                meeting.AttendeePassword
                );
    }
}
