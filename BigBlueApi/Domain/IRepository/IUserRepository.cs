namespace BigBlueApi.Domain.IRepository;
public interface IUserRepository
{
    Task<int> CreateUser(User user);
    Task EditUser(int id,User user);
}