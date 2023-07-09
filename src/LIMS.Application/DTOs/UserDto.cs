using BigBlueApi.Domain;
using LIMS.Domain;

namespace BigBlueApi.Application.DTOs;

public record UserAddEditDto(string FullName,string Alias,UserRoleTypes Role);