using BigBlueApi.Application.Models;
using BigBlueApi.Domain;

namespace BigBlueApi.Application.Mappers;

public static class ServerDtoMapper
{
    public static ServerAddEditDto Map(Server server) =>
        new ServerAddEditDto(server.ServerUrl, server.ServerLimit);

    public static Server Map(ServerAddEditDto server)
        => new Server(server.ServerUrl,server.ServerLimit);
}
