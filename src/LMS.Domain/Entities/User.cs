namespace LIMS.Domain.Entity;

public class User
{
    public int Id { get; }
    public string FullName { get; private set; }
    public string Alias { get; private set; }
    public UserRole Role { get; set; }
    public List<Session> Sessions { get; set; }

    public User(string fullName, string alias, UserRole role) =>
        (FullName, Alias, role) = (fullName, alias, role);

    private User() { }
}
