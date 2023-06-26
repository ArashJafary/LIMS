namespace BigBlueApi.Domain.IRepository;

public interface IMemberShipRepository
{
    Task<bool> CanJoinUser(Session session);
    Task<int> JoinUser(Session session, User user);
}
