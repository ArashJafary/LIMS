namespace BigBlueApi.Domain.IRepository;
public interface IUserRepository
{
    ValueTask<int> CreateUser(User user);
    Task EditUser(int id,User user);
}