namespace BigBlueApi.Domain;

public class MemberShip
{
    public int Id { get; set; }
    public User User { get; set; }
    public Server Server { get; set; }
    public DateTime JoinedDateTime { get; set; }
    public int LimitCapacity { get; set; }
    public List<Session> Sessions { get; set; }
}
