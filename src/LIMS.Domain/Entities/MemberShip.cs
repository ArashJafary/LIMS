namespace LIMS.Domain.Entities;

public class MemberShip : BaseEntity
{
    public Meeting Meeting { get; private set; }
    public User User { get; private set; }
    public DateTime JoinedDateTime { get; private set; }
    public bool UserRejected { get; private set; }
    public bool UserExited { get; private set; }

    public void BanUser()
        => UserRejected = true;

    private MemberShip() { }

    public MemberShip(long id, Meeting meeting, User user)
       => (Id, Meeting, User) = (id, meeting, user);

    public MemberShip(Meeting meeting, User user)
        => (Meeting, User) = (meeting, user);
}
