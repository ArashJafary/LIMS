namespace BigBlueApi.Domain.IRepository;
public interface ISessionRepository 
{
    Task<int> CreateSession(Session session);
    Task EditSession(int id,Session session);
}