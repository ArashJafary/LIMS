using LIMS.Application.DTOs;
using LIMS.Application.Mappers;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using LIMS.Application.Models;

namespace LIMS.Application.Services.Database.BBB
{
    public class BBBUserServiceImpl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _users;

        public BBBUserServiceImpl(IUnitOfWork unitOfWork, IUserRepository users) =>
            (_unitOfWork, _users) = (unitOfWork, users);

        public async ValueTask<OperationResult<long>> CreateUser(UserAddEditDto user)
        {
            try
            {
                var newUser = UserDtoMapper.Map(user);
                await _users.CreateUser(newUser);
                await _unitOfWork.SaveChangesAsync();
                if (newUser.Id == 0)
                    return OperationResult<long>.OnFailed("We have problem to create user in create");
                return OperationResult<long>.OnSuccess(newUser.Id);
            }
            catch (Exception ex)
            {
                return OperationResult<long>.OnException(ex);
            }
        }


        public async ValueTask<OperationResult> EditUser(long Id, UserAddEditDto userDto)
        {
            try
            {
                var user = await _users.GetUser(Id);
                user.UserUpdate(userDto.FullName, userDto.Alias,new UserRole(userDto.Role));
                await _unitOfWork.SaveChangesAsync();
                return new OperationResult(); 
            }
            catch (Exception ex)
            {
                return OperationResult.OnException(ex);
            }
        }

        public async ValueTask<OperationResult<UserAddEditDto>> GetUser(long userId)
        {
            try
            {
                var user = UserDtoMapper.Map(await _users.GetUser(userId));
                if (user is null)
                    return OperationResult<UserAddEditDto>.OnFailed("user not find");
                return OperationResult<UserAddEditDto>.OnSuccess(user);
            }
            catch (Exception ex)
            {
                return OperationResult<UserAddEditDto>.OnException(ex);
            }
        }


        public async ValueTask<OperationResult<List<UserAddEditDto>>> GetUsers()
        {
            try
            {
                var users= await _users.GetUsers();
                var userDto = new List<UserAddEditDto>();
                foreach (var User in users)
                {
                    userDto.Add(UserDtoMapper.Map(User));
                }
                return OperationResult<List<UserAddEditDto>>.OnSuccess(userDto);
            }
            catch(Exception ex)
            { 
                return OperationResult<List<UserAddEditDto>>.OnException(ex);
            }
        }

        public async ValueTask<OperationResult<long>> DeleteUser(long id)
        {
            try
            {
                var userId = await _users.DeleteUser(id);
                return OperationResult<long>.OnSuccess(userId);
            }
            catch (Exception ex)
            {
                return OperationResult<long>.OnException(ex);   
            }
        }
    }
}