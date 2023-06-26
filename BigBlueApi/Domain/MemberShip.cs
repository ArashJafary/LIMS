namespace BigBlueApi.Domain;

public class MemberShip
{
    public int Id { get; set; }
    public Session Session { get; set; }
    public User User { get; set; }
    public DateTime JoinedDateTime { get; set; }

}
