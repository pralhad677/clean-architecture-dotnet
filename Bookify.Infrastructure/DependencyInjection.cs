using Asp.Versioning;
using Bookify.Application.Abstraction.Caching;
using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Data;
using Bookify.Application.Abstraction.Email;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Caching;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.OutBox;
using Bookify.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Bookify.Infrastructure;

public static  class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
     // services.AddDbContext<ApplicationDbContext>(options =>
     //                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
     //            );


     //if not working -watch 42 number video and create DateOnlyTypeHnadler if needed
     var connectionString = configuration.GetConnectionString("DefaultConnection");
     services.AddSingleton<ISqlConnectionFactory>(_=>new SqlConnectionfactory(connectionString!));
     services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IApartmentsRepository, ApartmentRepository>();
    services.AddScoped<IBookingRepository, BookingRepository>();
    services.AddScoped<IUnitOfWork>(sp=>sp.GetRequiredService<ApplicationDbContext>());
    services.AddSingleton<IDateTimeProvider,DateTimeProvider>( );
    services.AddSingleton<IEmailService,EmailService>( );
    AddCaching(services,configuration);
    AddHealthChecks(services, configuration);
    ApiVersioning(services  );
    AddBackgroundJobs(services, configuration);
        return services;
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Cache")??
                               throw new ArgumentException(nameof(configuration));
        services.AddStackExchangeRedisCache(options=>options.Configuration=connectionString);
        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!); //AspNetCore.HealthChecks.SqlSer
    }

    private static void ApiVersioning(this IServiceCollection services)
    {
        // Asp.Versioning.Mvc -for controller --for api version
        // Asp.Versioning.Mvc.ApiExplorer  --so that swagger doesnt get confused with two same type of api
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V"; //v-> v of controller apiversion,V-> wildcard
                options.SubstituteApiVersionInUrl = true; // use dafultApiVersion from line 69as we hav e specfied, means only one
            });
    }

    private static void AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection( "Outbox"));
        services.AddQuartz();
        services.AddQuartzHostedService(options=>options.WaitForJobsToComplete = true);
        services.ConfigureOptions<ProcessOutBoxMessagesJobSetup>();
    }

}