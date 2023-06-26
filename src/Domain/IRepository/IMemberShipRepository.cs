namespace BigBlueApi.Domain.IRepository;

public interface IMemberShipRepository
{
    ValueTask<MemberShip> Find(int MemberShipID);
    ValueTask<int> JoinUser(Session session, User user);
}
