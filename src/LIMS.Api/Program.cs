using BigBlueButtonAPI.Core;
using LIMS.Persistence.Repositories;
using Hangfire;
using Hangfire.SqlServer;
using LIMS.Api.Extensions.Repositories;
using LIMS.Api.Extensions.Services.BBB;
using LIMS.Api.Extensions.Services.BBB.Database;
using LIMS.Api.Extensions.Services.Handlers;
using LIMS.Api.Extensions.Services.Schedulers;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Services.Http.BBB;
using LIMS.Application.Services.Meeting.BBB;
using LIMS.Application.Services.Schedulers.HangFire;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using LIMS.Infrastructure.Repositories;
using LIMS.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
    .AddBBBServices();

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
    .AddScoped<BBBServerActiveService>();

builder.Services
    .AddServiceHandler();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
