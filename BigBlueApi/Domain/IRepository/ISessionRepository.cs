namespace BigBlueApi.Domain.IRepository;
public interface ISessionRepository 
{
    ValueTask<string> CreateSession(Session session);
    ValueTask EditSession(string id,Session session);
    ValueTask StopRunnig(string id);
}