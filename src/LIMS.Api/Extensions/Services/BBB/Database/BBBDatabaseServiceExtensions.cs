using LIMS.Application.Services.Database.BBB;

namespace LIMS.Api.Extensions.Services.BBB.Database
{
    public static class BBBDatabaseServiceExtensions
    {
        public static IServiceCollection AddBBBServices(this IServiceCollection services)
        {
            services
                .AddScoped<BBBMeetingServiceImpl>();
            services
                .AddScoped<BBBMemberShipServiceImpl>();
            services
                .AddScoped<BBBRecordServiceImpl>();
            services
                .AddScoped<BBBServerServiceImpl>();
            services
                .AddScoped<BBBUserServiceImpl>();

            return services;
        }
    }
}
