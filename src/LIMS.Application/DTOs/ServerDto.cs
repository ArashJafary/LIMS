namespace LIMS.Application.DTOs;
public record ServerAddEditDto(string ServerUrl,string ServerSecret, int ServerLimit,bool IsActive);
