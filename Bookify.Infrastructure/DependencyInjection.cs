using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Data;
using Bookify.Application.Abstraction.Email;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    services.AddSingleton<IDateTimeProvider,IDateTimeProvider>( );
    services.AddSingleton<IEmailService,EmailService>( );
        return services;
    }

}