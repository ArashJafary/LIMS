using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DbSet<Session> _sessions;
        public SessionRepository(BigBlueContext context) => _sessions = context.Set<Session>();
        public async ValueTask<string> CreateSession(Session session)
        {
          var Session= await _sessions.AddAsync(session);
            return   Session.Entity.MeetingId;
        }

        public async ValueTask EditSession(string id, Session session)
        {
            var Seesion = await _sessions.FirstOrDefaultAsync(se => se.MeetingId == id);
            Seesion = session;
            Seesion.Update(session.MeetingId,session.Recorded,session.Name, session.ModeratorPassword, session.AttendeePassword);
            _sessions.Update(Seesion);
        }

        public async ValueTask StopRunnig(string id)
        {
            var Seesion = await _sessions.FirstOrDefaultAsync(se => se.MeetingId == id);
            Seesion!.IsRunning= false;
            _sessions.Update(Seesion);
        }
    }
}
