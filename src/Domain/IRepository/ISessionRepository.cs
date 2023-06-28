namespace BigBlueApi.Domain.IRepository;
public interface ISessionRepository 
{
    ValueTask<string> CreateSession(Session session);
    ValueTask<Session> Find(string meetingID);
    Task EditSession(string meetingId,Session session);
    Task EndSession(string meetingId);
}