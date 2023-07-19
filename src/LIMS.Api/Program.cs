using Hangfire;
using Hangfire.SqlServer;
using LIMS.Api.Extensions.Repositories;
using LIMS.Api.Extensions.Services.BBB;
using LIMS.Api.Extensions.Services.Bbb.Database;
using LIMS.Api.Extensions.Services.Handlers;
using LIMS.Api.Extensions.Services.Schedulers;
using LIMS.Application.Services.Http.BBB;
using LIMS.Application.Services.Schedulers.HangFire;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>().Build();
var connectionString = builder.Configuration.GetConnectionString("MSSQL");

builder.Services
    .AddControllers();
builder.Services
    .AddEndpointsApiExplorer();
builder.Services
    .AddSwaggerGen();

builder.Services
    .AddCustomHangfire(builder.Configuration);

builder.Services
    .AddBBBConfigurations(
        builder.Configuration);

builder.Services
    .AddBbbServices();

builder.Services
    .AddScoped<ServerSchedulerService>();

builder.Services
    .AddDbContext<LimsContext>(options =>
        options.UseSqlServer(connectionString));

builder.Services
    .AddScoped<IUnitOfWork, LimsContext>();

builder.Services
    .AddRepositories();

builder.Services
    .AddScoped<BbbServerActiveService>();

builder.Services
    .AddServiceHandler();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard(
    options: new DashboardOptions() { DashboardTitle = "Server Alive Schedulers" , IgnoreAntiforgeryToken = false },
    pathMatch: "/Scheduler", 
    storage: new SqlServerStorage(connectionString));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
