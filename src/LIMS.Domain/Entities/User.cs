using LIMS.Domain.Entities;

namespace LIMS.Domain.Entities;

public sealed class User : BaseEntity
{
    public string FullName { get; private set; }
    public string Alias { get; private set; }
    public UserRoleTypes Role { get; private set; }

    public IEnumerable<Meeting> Meetings { get; }

    public User(string fullName, string alias, UserRoleTypes role) =>
        (FullName, Alias, Role) = (fullName, alias, role);

    public void UserUpdate(string fullName, string alias, UserRoleTypes role) =>
      (FullName, Alias, Role) = (fullName, alias, role);

    private User() { }
}
