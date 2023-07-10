using LIMS.Application.DTOs;
using LIMS.Domain;
using LIMS.Domain;
using LIMS.Domain.Entities;
using LIMS.Domain.Entity;

namespace LIMS.Application.Mappers
{
    public static class UserDtoMapper
    {
        public static User Map(UserAddEditDto userDto) =>
            new User(userDto.FullName, userDto.Alias, new UserRole(userDto.Role));

        public static UserAddEditDto Map(User user) =>
            new UserAddEditDto(
                user.FullName,
                user.Alias,
                user.Role.RoleName switch
                {
                    "Moderator" => UserRoleTypes.Moderator,
                    "Attendee" => UserRoleTypes.Attendee,
                    "Guest" => UserRoleTypes.Guest
                }
            );
    }
}
