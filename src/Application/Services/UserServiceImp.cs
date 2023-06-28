using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Persistence.Repository;

namespace BigBlueApi.Application.Services
{
    public class UserServiceImp
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _Repository;

        public UserServiceImp(IUserRepository repository,IUnitOfWork uow)
        {
         _uow=uow;
         _Repository=repository;
        }


        public async ValueTask<int> CreateUser(User user)
        {
            var User = await _Repository.CreateUser(user);
            await _uow.SaveChangesAsync();
            return user.Id;
        }

        public async Task EditUser(int Id, User user)
        {
            var User = _Repository.EditUser(Id,user);
            await _uow.SaveChangesAsync();
        }

        public async ValueTask<User> Find(int userId)
        {
            var user = await _Repository.Find(userId);
            return user;
        }
    }
}
