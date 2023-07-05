using BigBlueApi.Application.DTOs;
using BigBlueApi.Application.Mappers;
using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Models;
using LIMS.Domain.Entity;
using LIMS.Domain.IRepositories;

namespace LIMS.Application.Services.Database.BBB
{
    public class BBBMeetingServiceImpl
    {
        private readonly IMeetingRepository _meetings;
        private readonly IUnitOfWork _uow;

        public BBBMeetingServiceImpl(IMeetingRepository repository, IUnitOfWork uow) =>
            (_meetings,_uow) = (repository, uow);

        public async ValueTask<OperationResult<long>> CreateMeetingAsync(SessionAddEditDto meeting)
        {
            await _meetings.CreateMeetingAsync(SessionMapper.Map(meeting));
            OperationResult<long>.OnSuccess(await _uow.SaveChangesAsync());
            return meeting.MeetingId;
        }

        public async ValueTask<bool> CanLogin(long id, UserRoleTypes role, string password)
        {
            var meeting = await _meetings sync(id);
            if (role == UserRoleTypes.Attendee)
                return meeting.AttendeePassword == password;
            else if (role == UserRoleTypes.Moderator)
                return meeting.ModeratorPassword == password;
            else
                return false;
        }

        public async ValueTask<Meeting> Find(long id) => await _meetings sync(id);

        public async ValueTask EditSession(long id, SessionAddEditDto session) =>
            await _meetings eMeetingAsync(id, SessionMapper.Map(session));

        public async ValueTask StopRunning(long id)
            => await _meetings etingAsync(id);
    }
}
