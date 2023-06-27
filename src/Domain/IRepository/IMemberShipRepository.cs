namespace BigBlueApi.Domain.IRepository;

public interface IMemberShipRepository
{
    ValueTask<bool> CanJoinUserOnSession(int MemberShipID);
    ValueTask<int> JoinUser(Session session, User user);
}
