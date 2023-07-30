using LIMS.Application.DTOs;
using LIMS.Application.Mappers;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Application.Models;
using Microsoft.Extensions.Logging;
using System;

namespace LIMS.Application.Services.Database.BBB
{
    public class BbbUserServiceImpl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _users;
        private readonly ILogger<BbbServerServiceImpl> _logger;


        public BbbUserServiceImpl(IUnitOfWork unitOfWork,
            IUserRepository users,
            ILogger<BbbServerServiceImpl> logger) =>
            (_unitOfWork, _users, _logger) = (unitOfWork, users, logger);

        public async ValueTask<OperationResult<long>> CreateNewUser(UserAddEditDto user)
        {
            try
            {
                var newUser = UserDtoMapper.Map(user);

                await _users.CreateAsync(newUser);

                await _unitOfWork.SaveChangesAsync();

                return OperationResult<long>.OnSuccess(newUser.Id);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<long>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult> EditExistedUser(long Id, UserAddEditDto userDto)
        {
            try
            {
                var user = await _users.GetByIdAsync(Id);

                user.UserUpdate(userDto.FullName, userDto.Alias, new UserRole(userDto.Role));

                await _unitOfWork.SaveChangesAsync();

                return new OperationResult();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<UserAddEditDto>> GetUserById(long userId)
        {
            try
            {
                var user = UserDtoMapper.Map(await _users.GetByIdAsync(userId));

                if (user is null)
                    return OperationResult<UserAddEditDto>.OnFailed("User Not Found.");

                return OperationResult<UserAddEditDto>.OnSuccess(user);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<UserAddEditDto>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<List<UserAddEditDto>>> GetAllUsers()
        {
            try
            {
                var users = await _users.GetAllAsync();

                var userDto = new List<UserAddEditDto>();

                foreach (var User in users)
                    userDto.Add(UserDtoMapper.Map(User));

                return OperationResult<List<UserAddEditDto>>.OnSuccess(userDto);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<List<UserAddEditDto>>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<long>> DeleteOneUser(long id)
        {
            try
            {
                var userId = await _users.DeleteAsync(id);

                await _unitOfWork.SaveChangesAsync();

                return OperationResult<long>.OnSuccess(userId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return OperationResult<long>.OnException(exception);
            }
        }
    }
}