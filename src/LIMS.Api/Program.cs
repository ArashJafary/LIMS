using Hangfire;
using Hangfire.SqlServer;
using LIMS.Api.Extensions.Repositories;
using LIMS.Api.Extensions.Services.BBB;
using LIMS.Api.Extensions.Services.Bbb.Database;
using LIMS.Api.Extensions.Services.Handlers;
using LIMS.Api.Extensions.Services.Schedulers;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using LIMS.Api.Middlewares;
using Serilog;
using LIMS.Application.Services.Http;
using LIMS.Application.Services.Schedulers;
using LIMS.Application.Services.Interfaces;
using LIMS.Infrastructure.ExternalApi.BBB;
using LIMS.Application.Strategy;
using LIMS.Domain.Services;
using LIMS.Application.Services.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>().Build();

var hangfireConnectionString = builder.Configuration.GetConnectionString("Hangfire");

var mssqlConnectionString = builder.Configuration.GetConnectionString("MSSQL");

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();

builder.Services.AddScoped<IHandleMeetingService, BbbHandleMeetingService>();

builder.Services.AddScoped<PlatformHandleResolverService, PlatformHandleResolverService>();

builder.Services.AddScoped<List<IHandleMeetingService>>(handlerProvider =>
{
    List<IHandleMeetingService> services = new();

    foreach (var service in handlerProvider.GetServices<IHandleMeetingService>())
        services.Add(service);

    return services;
});

builder.Services.AddScoped<IConnectionService, BbbConnectionService>();

builder.Logging.ClearProviders();

builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCustomHangfire(builder.Configuration);

builder.Services.AddBBBConfigurations(builder.Configuration);

builder.Services.AddImplementationServices();

builder.Services.AddScoped<HangfireSchedulerService>();

builder.Services.AddDbContext<LimsContext>(options => options.UseSqlServer(mssqlConnectionString));

builder.Services.AddScoped<IUnitOfWork, LimsContext>();

builder.Services.AddRepositories();

builder.Services.AddScoped<ServerActiveService>();

builder.Services.AddServiceHandler();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();

app.UseHangfireDashboard(
    options: new DashboardOptions() { DashboardTitle = "Server Alive Schedulers", IgnoreAntiforgeryToken = false },
    pathMatch: "/Scheduler",
    storage: new SqlServerStorage(hangfireConnectionString));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
