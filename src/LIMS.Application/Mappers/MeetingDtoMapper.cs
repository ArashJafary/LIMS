using LIMS.Application.DTOs;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;

namespace LIMS.Application.Mappers
{
    public static class MeetingDtoMapper
    {
        public static Meeting Map(MeetingAddDto meetingDto) =>
            new Meeting(meetingDto.MeetingId,
                meetingDto.IsRecord,
                meetingDto.Name,
                meetingDto.ModeratorPassword,
                meetingDto.AttendeePassword,
                meetingDto.StartDateTime,
                meetingDto.EndDateTime,
                meetingDto.LimitCapacity,
                meetingDto.ParentId,
                true,
                meetingDto.IsBreakout,
                ServerDtoMapper.Map(meetingDto.Server),
                meetingDto.AutoStartRecord,
                meetingDto.Platform);

        public static MeetingAddDto Map(Meeting meeting) =>
            new MeetingAddDto(
                meeting.IsRecord,
                meeting.Name,
                meeting.ModeratorPassword,
                meeting.AttendeePassword,
                meeting.StartDateTime,
                meeting.EndDateTime,
                meeting.LimitCapacity,
                ServerDtoMapper.Map(meeting.Server),
                meeting.AutoStartRecording,
                meeting.ParentMeetingId,
            );

        public static Meeting MapEditDto(MeetingEditDto meetingDto) =>
            new Meeting(
                meetingDto.MeetingId,
                meetingDto.IsRecord,
                meetingDto.Name,
                meetingDto.ModeratorPassword,
                meetingDto.AttendeePassword
            );

        public static MeetingEditDto MapEditDto(Meeting meeting) =>
            new MeetingEditDto(
                meeting.MeetingId,
                meeting.IsRecord,
                meeting.Name,
                meeting.ModeratorPassword,
                meeting.AttendeePassword,
                meeting.EndDateTime,
                meeting.LimitCapacity
            );
    }
}
