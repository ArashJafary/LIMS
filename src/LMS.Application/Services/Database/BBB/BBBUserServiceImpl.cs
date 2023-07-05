using BigBlueApi.Application.DTOs;
using BigBlueApi.Application.Mappers;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Persistence.Repository;
using LIMS.Domain.Entity;

namespace LIMS.Application.Services.Database.BBB
{
    public class BBBUserServiceImpl
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _Repository;

        public BBBUserServiceImpl(IUserRepository repository, IUnitOfWork uow)
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
