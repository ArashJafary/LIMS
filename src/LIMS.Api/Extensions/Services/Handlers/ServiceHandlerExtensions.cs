using LIMS.Application.Services.Meeting.BBB;

namespace LIMS.Api.Extensions.Services.Handlers
{
    public static class ServiceHandlerExtensions
    {
        public static IServiceCollection AddServiceHandler(this IServiceCollection services)
        {
            services
                .AddScoped<BBBHandleMeetingService>();
            return services;
        }
    }
}
