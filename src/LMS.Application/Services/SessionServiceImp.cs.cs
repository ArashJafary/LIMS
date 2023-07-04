using BigBlueApi.Application.DTOs;
using BigBlueApi.Application.Mappers;
using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Persistence.Repository;
using LIMS.Domain.Entity;
using LIMS.Domain.IRepositories;

namespace BigBlueApi.Application.Services
{
    public class SessionServiceImp
    {
        private readonly IMeetingRepository _repository;
        private readonly IUnitOfWork _uow;

        public SessionServiceImp(IMeetingRepository repository, IUnitOfWork uow) =>
            (_repository, _uow) = (repository, uow);

        public async ValueTask<string> CreateMeetingAsync(SessionAddEditDto sessionDto)
        {
            await _repository.CreateMeetingAsync(SessionMapper.Map(sessionDto));
            await _uow.SaveChangesAsync();
            return sessionDto.MeetingId;
        }

        public async ValueTask<bool> CanLogin(long id, UserRoleTypes role, string password)
        {
            var meeting = await _repository.FindAsync(id);
            if (role == UserRoleTypes.Attendee)
                return meeting.AttendeePassword == password;
            else if (role == UserRoleTypes.Moderator)
                return meeting.ModeratorPassword == password;
            else
                return false;
        }

        public async ValueTask<Meeting> Find(long id) => await _repository.FindAsync(id);

        public async ValueTask EditSession(long id, SessionAddEditDto session) =>
            await _repository.UpdateMeetingAsync(id, SessionMapper.Map(session));

        public async ValueTask StopRunning(long id) 
            => await _repository.EndMeetingAsync(id);
    }
}
