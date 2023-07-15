using BigBlueButtonAPI.Core;
using Microsoft.Extensions.Options;

namespace LIMS.Api.Extensions.Services.BBB
{
    public static class BBBServicesExtensions
    {
        public static IServiceCollection AddBBBConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions();

            services
                .AddHttpClient();

            services.Configure<BigBlueButtonAPISettings>(
                configuration
                    .GetSection("BBBSettings"));

            services.AddScoped<BigBlueButtonAPIClient>(provider =>
                new BigBlueButtonAPIClient(
                    provider
                        .GetRequiredService<IOptions<BigBlueButtonAPISettings>>()
                        .Value,
                    provider
                        .GetRequiredService<IHttpClientFactory>()
                        .CreateClient()));

            return services;
        }
    }
}
