using LIMS.Domain;
using LIMS.Domain;

namespace LIMS.Application.DTOs;
public record UserAddEditDto(string FullName,string Alias,UserRoleTypes Role);