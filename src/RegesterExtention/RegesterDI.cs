using BigBlueApi.Application.Services;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Persistence;
using BigBlueApi.Persistence.Repository;

namespace BigBlueApi.RegesterExtention
{
    public static class RegesterDI
    {
        public static IServiceCollection RegesterDi(this IServiceCollection Services)
        {
            Services.AddScoped<ISessionRepository, SessionRepository>();
            Services.AddScoped<IUserRepository, UserRepository>();
            Services.AddScoped<IMemberShipRepository, MemberShipRepository>();
            Services.AddScoped<IServerRepository, ServerRepository>();
            Services.AddScoped<IUnitOfWork, BigBlueContext>();
            Services.AddScoped<SessionServiceImp>();
            Services.AddScoped<ServerServiceImp>();
            Services.AddScoped<UserServiceImp>();
            Services.AddScoped<MemberShipServiceImp>();
            return Services;
        }
    }
}
