using LIMS.Persistence.Repositories;
using Hangfire;
using Hangfire.SqlServer;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Services.Http.BBB;
using LIMS.Application.Services.Meeting.BBB;
using LIMS.Application.Services.Schedulers.HangFire;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using LIMS.Infrastructure.Repositories;
using LIMS.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUnitOfWork, LimsContext>();

builder.Services.AddDbContext<LimsContext>(options =>
{
    options.UseSqlServer();
});

builder.Services
    .AddScoped<IMeetingRepository, MeetingRepository>();
builder.Services
    .AddScoped<IServerRepository, ServerRepository>();
builder.Services
    .AddScoped<IUserRepository, UserRepository>();
builder.Services
    .AddScoped<IRecordRepository, RecordRepository>();
builder.Services
    .AddScoped<IMemberShipRepository, MemberShipRepository>();

builder.Services
    .AddScoped<BBBMeetingServiceImpl>();
builder.Services
    .AddScoped<BBBMemberShipServiceImpl>();
builder.Services
    .AddScoped<BBBRecordServiceImpl>();
builder.Services
    .AddScoped<BBBServerServiceImpl>();
builder.Services
    .AddScoped<BBBUserServiceImpl>();

builder.Services.AddScoped<BBBServerActiveService>();

builder.Services
    .AddScoped<BBBHandleMeetingService>();

builder.Services
    .AddScoped<BBBMeetingServiceImpl>();

builder.Services
    .AddSingleton<ServerSchedulerService>();

builder.Services.AddHangfire(configuration 
    => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(
        builder.Configuration
            .GetConnectionString("Hangfire"), 
                new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                }));

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
