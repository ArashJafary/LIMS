using BigBlueApi.Application.DTOs;
using BigBlueApi.Application.Mappers;
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
            _uow = uow;
            _Repository = repository;
        }

        public async ValueTask<int> CreateUser(UserAddEditDto user)
        {
            var newUser = UserDtoMapper.Map(user);
            await _Repository.CreateUser(newUser);
            await _uow.SaveChangesAsync();
            return newUser.Id;
        }

        public async Task EditUser(int Id, UserAddEditDto user)
        {
            var newUser = UserDtoMapper.Map(user);
            await _Repository.EditUser(Id, newUser);
            await _uow.SaveChangesAsync();
        }

        public async ValueTask<User> Find(int userId) => await _Repository.Find(userId);
    }
}
