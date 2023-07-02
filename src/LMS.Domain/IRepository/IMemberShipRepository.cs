namespace BigBlueApi.Domain.IRepository;

public interface IMemberShipRepository
{
    ValueTask<bool> CanJoinUserOnSession(string meetingId);
    ValueTask<int> JoinUser(Session session, User user);
}
