using BigBlueApi.Application.Services;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Persistence;
using BigBlueApi.Persistence.Repository;
using BigBlueApi.RegesterExtention;
using BigBlueButtonAPI.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BigBlueContext>(
    options =>
        options
            .UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);
builder.Services.RegesterDi();

builder.Services.AddOptions();
builder.Services.AddHttpClient();

builder.Services.Configure<BigBlueButtonAPISettings>(
    builder.Configuration.GetSection("BigBlueButtonApiSettings")
);
builder.Services.AddScoped<BigBlueButtonAPIClient>(provider =>
{
    var settings = provider.GetRequiredService<IOptions<BigBlueButtonAPISettings>>().Value;
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    return new BigBlueButtonAPIClient(settings, factory.CreateClient());
});
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
