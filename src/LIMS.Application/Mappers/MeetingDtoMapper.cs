using LIMS.Application.DTOs;
using LIMS.Domain.Entities;

namespace LIMS.Application.Mappers
{
    public static class MeetingDtoMapper
    {
        public static async Task<Meeting> MapAsync(MeetingAddDto meetingDto)
        {
            var meeting = new Meeting(
                meetingDto.MeetingId,
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

            meeting.CanFreeJoinOnBreakout(meetingDto.CanFreeJoinOnBreakout);

            return meeting;
        }
    }
}
