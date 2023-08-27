using LIMS.Application.DTOs;
using LIMS.Application.Mappers;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Application.Models;
using Microsoft.Extensions.Logging;
using System;

namespace LIMS.Application.Services.Database
{
    public class UserServiceImpl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _users;
        private readonly ILogger<ServerServiceImpl> _logger;

        public UserServiceImpl(IUnitOfWork unitOfWork,
            IUserRepository users,
            ILogger<ServerServiceImpl> logger) =>
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
            var user = await _users.GetByIdAsync(Id);

            user.UserUpdate(userDto.FullName, userDto.Alias, userDto.Role);

            await _unitOfWork.SaveChangesAsync();

            return new OperationResult();
        }

        public async ValueTask<OperationResult<UserAddEditDto>> GetUserById(long userId)
        {
            var user = UserDtoMapper.Map(await _users.GetByIdAsync(userId));

            if (user is null)
                return OperationResult<UserAddEditDto>.OnFailed("User Not Found.");

            return OperationResult<UserAddEditDto>.OnSuccess(user);
        }

        public async ValueTask<OperationResult<List<UserAddEditDto>>> GetAllUsers()
        {
            var users = await _users.GetAllAsync();

            var userDto = new List<UserAddEditDto>();

            foreach (var User in users)
                userDto.Add(UserDtoMapper.Map(User));

            return OperationResult<List<UserAddEditDto>>.OnSuccess(userDto);
        }

        public async ValueTask<OperationResult<long>> DeleteOneUser(long id)
        {
            var userId = await _users.DeleteAsync(id);

            await _unitOfWork.SaveChangesAsync();

            return OperationResult<long>.OnSuccess(userId);
        }
    }
}