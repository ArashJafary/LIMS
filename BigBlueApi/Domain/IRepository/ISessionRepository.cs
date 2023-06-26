namespace BigBlueApi.Domain.IRepository;
public interface ISessionRepository 
{
    ValueTask<string> CreateSession(Session session);
    ValueTask<Session> Find(string meetingID);
    Task EditSession(string id,Session session);
    Task StopRunnig(string id);
}