using Bookify.Application.Abstraction.Behavious;
using Bookify.Application.Abstraction.Clock;
using Bookify.Domain.Bookings;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace Bookify.Application;

public static  class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
    config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient<PricingService>();
        return services;
    }
    
}