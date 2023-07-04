using LIMS.Domain.Entity;

namespace BigBlueApi.Domain.IRepository;
public interface IUserRepository
{
    ValueTask<long> CreateUser(User user);
    ValueTask<User> GetUser(long userId);
    ValueTask<IEnumerable<User>> GetAllUsers();
    Task<long> DeleteUser(long userId);
    Task EditUser(long id, User user);
}