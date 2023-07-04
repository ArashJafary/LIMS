using LIMS.Domain.Entity;

namespace BigBlueApi.Domain.IRepository;
public interface IUserRepository
{
    ValueTask<int> CreateUser(User user);
    ValueTask<User> Find(int userId);
    Task EditUser(int id,User user);
}