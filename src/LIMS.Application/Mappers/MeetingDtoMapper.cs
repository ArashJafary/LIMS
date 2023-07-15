using LIMS.Application.DTOs;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;

namespace LIMS.Application.Mappers
{
    public static class MeetingDtoMapper
    {
        public static async Task<Meeting> Map(MeetingAddDto meetingDto)
        {
            var meeting =new Meeting(meetingDto.MeetingId,
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
            await meeting.CanFreeJoinOnBreakout(meetingDto.CanFreeJoinOnBreakout);

            return meeting;
        }


        public static async Task<MeetingAddDto> Map(Meeting meeting)
            => await Task.Run(() =>
            {
                var meetingDto = new MeetingAddDto(
                    meeting.MeetingId,
                    meeting.IsRecord,
                    meeting.IsBreakout,
                    meeting.FreeJoinOnBreakout,
                    meeting.ParentMeetingId,
                    meeting.Name,
                    meeting.ModeratorPassword,
                    meeting.AttendeePassword,
                    meeting.StartDateTime,
                    meeting.EndDateTime,
                    meeting.LimitCapacity,
                    ServerDtoMapper.Map(meeting.Server),
                    meeting.AutoStartRecording,
                    meeting.Platform
                );

                return meetingDto;
            });
          

        public static Meeting MapEditDto(MeetingEditDto meetingDto) =>
            new Meeting(
                meetingDto.MeetingId,
                meetingDto.IsRecord,
                meetingDto.Name,
                meetingDto.ModeratorPassword,
                meetingDto.AttendeePassword,
                meetingDto.EndDateTime,
                meetingDto.limitCapacity,
                meetingDto.CanFreeJoinOnBreakout
            );

        public static MeetingEditDto MapEditDto(Meeting meeting) =>
            new MeetingEditDto(
                meeting.MeetingId,
                meeting.IsRecord,
                meeting.Name,
                meeting.ModeratorPassword,
                meeting.AttendeePassword,
                meeting.EndDateTime,
                meeting.LimitCapacity,
                meeting.FreeJoinOnBreakout
            );
    }
}
