namespace BigBlueApi.Domain;

public class User
{
    public int Id { get; }
    public string FullName { get; private set; }
    public string Alias { get; private set; }
    public UserRole Role { get; set; }
    public List<Session> Sessions { get; set; }

    public User(string fullName, UserRole role,string alias) => (FullName, Role,Alias) = (fullName, role,alias);

    private User() { }
}
