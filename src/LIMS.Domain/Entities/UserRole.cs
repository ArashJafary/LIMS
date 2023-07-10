using LIMS.Domain;
using LIMS.Domain.Entities;
using LIMS.Domain.Entity;

namespace LIMS.Domain.Entities;

public sealed class UserRole : BaseEntity
{
    public string RoleName { get; private set; }
    public IEnumerable<User> Users { get; }
    public UserRole(UserRoleTypes role)
        => RoleName = role.ToString();
    private UserRole() { }
}
