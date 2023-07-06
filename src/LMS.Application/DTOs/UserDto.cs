using BigBlueApi.Domain;

namespace BigBlueApi.Application.DTOs;

public record UserAddEditDto(string FullName,string Alias,UserRoleTypes Role);