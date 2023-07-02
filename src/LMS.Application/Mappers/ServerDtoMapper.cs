using BigBlueApi.Application.DTOs;
using BigBlueApi.Domain;

public static class ServerDtoMapper
{
    public static ServerAddEditDto Map(Server server) =>
        new ServerAddEditDto(server.ServerUrl, server.SharedSecret, server.ServerLimit);

    public static Server Map(ServerAddEditDto server) =>
        new Server(server.ServerUrl, server.ServerLimit);
}