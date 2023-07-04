using BigBlueApi.Domain;

namespace LIMS.Domain.Entity;

public class UserRole
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public List<User> Users { get; set; }

    public UserRole(UserRoles role)
    {
        RoleName = role.ToString();
    }

    private UserRole() { }
}
