using Bookify.Application.Abstraction.Behaviours;
using Bookify.Application.Abstraction.Behavious;
using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Messaging;
using Bookify.Application.Apartments;
using Bookify.Application.Apartments.SearchApartments;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using FluentValidation;
using MediatR;
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
                config.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
        });
        // services.AddTransient < IRequestHandler<SearchApartmentQuery, IReadOnlyList<ApartmentResponse>>;
        // services.AddTransient<IRequestHandler<SearchApartmentQuery,IReadOnlyList<ApartmentResponse>>>();
        // services.AddTransient<IRequestHandler<SearchApartmentQuery, IReadOnlyList<ApartmentResponse>>, SearchApartmentQueryHandler>();
        // services.AddTransient<IRequestHandler<SearchApartmentQuery, Result<IReadOnlyList<ApartmentResponse>>>, SearchApartmentQueryHandler>();

        services.AddScoped<IQueryHandler<SearchApartmentQuery, Result<IReadOnlyList<ApartmentResponse>>>, SearchApartmentQueryHandler>();

        services.AddScoped<IQueryHandler<GetApartmentQuery,Result< Guid>>,GetApartmentQueryHandler>();
        // services.AddTransient<IRequestHandler<SearchApartmentQuery, Result<IReadOnlyList<ApartmentResponse>>>, SearchApartmentQueryHandler>();
    // services.AddTransient<IRequestHandler<SearchApartmentQuery, Result<IReadOnlyList<ApartmentResponse>>>, SearchApartmentQueryHandler>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient<PricingService>();
        return services;
    }
    
}