using LIMS.Domain.Entity;

namespace BigBlueApi.Domain.IRepository;
public interface IUserRepository
{
    ValueTask<long> CreateUser(User User);
    ValueTask<User> GetUser(long UserId);
    ValueTask<IEnumerable<User>> GetUsers();
    Task<long> DeleteUser(long UserId);
    Task EditUser(long Id, User User);
}