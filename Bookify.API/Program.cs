using Bookify.API.Extensions;
using Bookify.API.OpenApi;
using Bookify.Application;
using Bookify.Application.Abstraction.Data;
using Bookify.Infrastructure;
using Dapper;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
;builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//AspNetCore.HealthChecks.UI.Client ---pacakge
// builder.Services.AddHealthChecks().AddCheck<CustomHealthCheck>("Custom-health-check"); dont use custom health checks


builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigration();
    app.SeedData();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            var ur = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(ur, name);
        }
    });
}

app.UseHttpsRedirection();
app.UseRequestContextLogging();
app.UseSerilogRequestLogging();
app.UseCustomExceptionHandler();
app.MapControllers();

app.MapHealthChecks("/health",new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();

// public class CustomHealthCheck(ISqlConnectionFactory sqlConnectionFactory) : IHealthCheck
// {
//
//     public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
//         CancellationToken cancellationToken = new CancellationToken())
//     {
//         try
//         {
//
//
//             using var connection = sqlConnectionFactory.CreateConnection();
//             await connection.ExecuteAsync("Select 1;");
//             return HealthCheckResult.Healthy();
//         }
//         catch (Exception ex)
//         {
//             return HealthCheckResult.Unhealthy(ex.Message);
//         }
//     }
// }