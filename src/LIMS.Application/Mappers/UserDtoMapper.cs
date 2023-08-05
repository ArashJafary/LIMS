using LIMS.Application.DTOs;
using LIMS.Domain;
using LIMS.Domain;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;

namespace LIMS.Application.Mappers
{
    public static class UserDtoMapper
    {
        public static User Map(UserAddEditDto userDto) =>
            new User(userDto.FullName, userDto.Alias, userDto.Role);

        public static UserAddEditDto Map(User user) =>
            new UserAddEditDto(
                user.FullName,
                user.Alias,
                user.Role
            );
    }
}
