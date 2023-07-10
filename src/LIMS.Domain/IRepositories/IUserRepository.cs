using LIMS.Domain.Entities;
using LIMS.Domain.Entity;

namespace LIMS.Domain.IRepositories;
public interface IUserRepository
{
    ValueTask<long> CreateUser(User User);
    ValueTask<User> GetUser(long UserId);
    ValueTask<IEnumerable<User>> GetUsers();
    Task<long> DeleteUser(long UserId);
}