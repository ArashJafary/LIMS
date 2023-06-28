using BigBlueApi.Application.DTOs;
using BigBlueApi.Application.Mappers;
using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Persistence.Repository;

namespace BigBlueApi.Application.Services
{
    public class SessionServiceImp
    {
        private readonly ISessionRepository _repository;
        private readonly IUnitOfWork _uow;

        public SessionServiceImp(ISessionRepository repository, IUnitOfWork uow) =>
            (_repository, _uow) = (repository, uow);

        public async ValueTask<string> CreateSession(SessionAddEditDto sessionDto)
        {
            await _repository.CreateSession(SessionMapper.Map(sessionDto));
            await _uow.SaveChangesAsync();
            return sessionDto.MeetingId;
        }

        public async ValueTask<bool> CanLogin(string meetindId, UserRoles role, string password)
        {
            var session = await _repository.Find(meetindId);
            if (role == UserRoles.Attendee)
                return session.AttendeePassword == password;
            else if (role == UserRoles.Moderator)
                return session.ModeratorPassword == password;
            else
                return true;
        }

        public async ValueTask<Session> Find(string meetingID) => await _repository.Find(meetingID);

        public async ValueTask EditSession(string id, SessionAddEditDto session) =>
            await _repository.EditSession(id, SessionMapper.Map(session));

        public async ValueTask StopRunning(string id) => await _repository.EndSession(id);
    }
}
