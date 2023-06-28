using BigBlueApi.Application.DTOs;
using BigBlueApi.Domain;

namespace BigBlueApi.Application.Mappers
{
    public static class UserDtoMapper
    {
        public static User Map(UserAddEditDto userDto) =>
            new User(userDto.FullName, userDto.Alias, userDto.Role);

        public static UserAddEditDto Map(User user) =>
            new UserAddEditDto(user.FullName, user.Alias, user.Role);
    }
}
