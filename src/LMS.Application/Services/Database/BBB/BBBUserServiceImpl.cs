using BigBlueApi.Application.DTOs;
using BigBlueApi.Application.Mappers;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Models;
using LIMS.Domain.Entity;

namespace LIMS.Application.Services.Database.BBB
{
    public class BBBUserServiceImpl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _Repository;

        public BBBUserServiceImpl(IUserRepository repository, IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _Repository = repository;
        }

        public async ValueTask<OperationResult<long>> CreateUser(UserAddEditDto User)
        {
            try
            {
                var NewUser = UserDtoMapper.Map(User);
                await _Repository.CreateUser(NewUser);
                await _unitOfWork.SaveChangesAsync();
                if (NewUser.Id == 0)
                    return OperationResult<long>.OnFailed("We have proplem to creat user in creat");
                return OperationResult<long>.OnSuccess(NewUser.Id);
            }
            catch (Exception ex)
            {
                return OperationResult<long>.OnException(ex);
            }
        }


        public async ValueTask<OperationResult> EditUser(int Id, UserAddEditDto User)
        {
            try
            {
                await _Repository.EditUser(Id,UserDtoMapper.Map(User));
                await _unitOfWork.SaveChangesAsync();
                return new OperationResult(); 
            }
            catch (Exception ex)
            {
                return OperationResult.OnException(ex);
            }
        }

        public async ValueTask<OperationResult<UserAddEditDto>> GetUser(int UserId)
        {
            try
            {
                var User = UserDtoMapper.Map(await _Repository.GetUser(UserId));
                if (User is null)
                    return OperationResult<UserAddEditDto>.OnFailed("user not find");
                return OperationResult<UserAddEditDto>.OnSuccess(User);
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
                var Users= await _Repository.GetUsers();
                var UserDto = new List<UserAddEditDto>();
                foreach (var User in Users)
                {
                    UserDto.Add(UserDtoMapper.Map(User));
                }
                return OperationResult<List<UserAddEditDto>>.OnSuccess(UserDto);
            }
            catch(Exception ex)
            { 
                return OperationResult<List<UserAddEditDto>>.OnException(ex);
            }
        }

        public async ValueTask<OperationResult<long>> DeleteUser(long Id)
        {
            try
            {
                var UserId = await _Repository.DeleteUser(Id);
                return OperationResult<long>.OnSuccess(UserId);
            }
            catch (Exception ex)
            {
                return OperationResult<long>.OnException(ex);   
            }
        }
    }
}