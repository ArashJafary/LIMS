using LIMS.Application.Services.Database;
using LIMS.Application.Services.Http;
using LIMS.Application.Services.Interfaces;
using LIMS.Infrastructure.ExternalApi.BBB;

namespace LIMS.Api.Extensions.Services.Bbb.Database
{
    public static class BbbDatabaseServiceExtensions
    {
        public static IServiceCollection AddImplementationServices(this IServiceCollection services)
        {
            services
                .AddScoped<MeetingServiceImpl>();
            services
                .AddScoped<MemberShipServiceImpl>();
            services
                .AddScoped<RecordServiceImpl>();
            services
                .AddScoped<ServerServiceImpl>();
            services
                .AddScoped<UserServiceImpl>();

            services
                .AddScoped<MeetingSettingsService>();

            return services;
        }
    }
}
