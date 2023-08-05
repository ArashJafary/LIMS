using LIMS.Application.Services.Handlers;
using LIMS.Domain.Services;

namespace LIMS.Api.Extensions.Services.Handlers
{
    public static class ServiceHandlerExtensions
    {
        public static IServiceCollection AddServiceHandler(this IServiceCollection services)
        {
            services
                .AddScoped<IHandleMeetingService,BbbHandleMeetingService>();

            return services;
        }
    }
}
