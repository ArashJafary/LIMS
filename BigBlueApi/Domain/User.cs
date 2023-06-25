namespace BigBlueApi.Domain;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Alias { get; set; }
    public UserRole Role { get; set; }
    public List<Session> Sessions { get; set; }

    public User(string fullName, UserRole role,string alias) => (FullName, Role,Alias) = (fullName, role,alias);

    private User() { }
}
