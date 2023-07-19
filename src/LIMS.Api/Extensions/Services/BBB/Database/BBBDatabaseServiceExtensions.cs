using LIMS.Application.Services.Database.BBB;
using LIMS.Infrastructure.ExternalApi.BBB;

namespace LIMS.Api.Extensions.Services.Bbb.Database
{
    public static class BbbDatabaseServiceExtensions
    {
        public static IServiceCollection AddBbbServices(this IServiceCollection services)
        {
            services
                .AddScoped<BbbConnectionService>();
            services
                .AddScoped<BbbMeetingServiceImpl>();
            services
                .AddScoped<BbbMemberShipServiceImpl>();
            services
                .AddScoped<BbbRecordServiceImpl>();
            services
                .AddScoped<BbbServerServiceImpl>();
            services
                .AddScoped<BbbUserServiceImpl>();

            return services;
        }
    }
}
