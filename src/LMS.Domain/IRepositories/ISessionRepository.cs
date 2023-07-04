using LIMS.Domain.Entity;

namespace BigBlueApi.Domain.IRepository;
public interface ISessionRepository 
{
    ValueTask<string> CreateSession(Session session);
    ValueTask<Session> Find(string meetingID);
    ValueTask EditSession(string meetingId,Session session);
    ValueTask EndSession(string meetingId);
}