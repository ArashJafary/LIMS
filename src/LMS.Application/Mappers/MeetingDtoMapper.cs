using BigBlueApi.Application.DTOs;
using LIMS.Domain.Entities;
using LIMS.Domain.Entity;

namespace BigBlueApi.Application.Mappers
{
    public static class MeetingDtoMapper
    {
        public static Meeting Map(MeetingAddDto meetingDto) =>
            new Meeting(
                 meetingDto.MeetingId,
                 meetingDto.IsRecord,
                 meetingDto.Name,
                 meetingDto.ModeratorPassword,
                 meetingDto.AttendeePassword
                );

        public static MeetingAddDto Map(Meeting meeting) =>
            new MeetingAddDto(
                meeting.MeetingId,
                meeting.IsRecord,
                meeting.Name,
                meeting.ModeratorPassword,
                meeting.AttendeePassword
                );
    }
}
