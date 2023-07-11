using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Repositories;
using LIMS.Persistence.Repositories;

namespace LIMS.Api.Extensions.Repositories
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IMeetingRepository, MeetingRepository>();
            services
                .AddScoped<IServerRepository, ServerRepository>();
            services
                .AddScoped<IUserRepository, UserRepository>();
            services
                .AddScoped<IRecordRepository, RecordRepository>();
            services
                .AddScoped<IMemberShipRepository, MemberShipRepository>();

            return services;
        }
    }
}
