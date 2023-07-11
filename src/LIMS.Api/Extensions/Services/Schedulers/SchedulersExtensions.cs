using Hangfire;
using Hangfire.SqlServer;

namespace LIMS.Api.Extensions.Services.Schedulers
{
    public static class SchedulersExtensions
    {
        public static IServiceCollection AddCustomHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(provider =>
                provider.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(
                        configuration.GetConnectionString("Hangfire"),
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                        }));

            return services;
        }
    }
}
