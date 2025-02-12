using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Exception;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.OutBox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bookify.Infrastructure;

public sealed  class ApplicationDbContext:DbContext,IUnitOfWork
{


    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };
    private readonly IDateTimeProvider _dateTimeProvider;
    public ApplicationDbContext(DbContextOptions options,   IDateTimeProvider dateTimeProvider)
        : base(options)
    {

        _dateTimeProvider = dateTimeProvider;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly( typeof( ApplicationDbContext).Assembly );
        base.OnModelCreating(modelBuilder);
    }

    public override async  Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {

            AddDomainEventsAsOutBoxMessages();
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concureency Exception Occurs",ex);
        }
    }

    private void AddDomainEventsAsOutBoxMessages()
    {
        var outBoxMessages = this.ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvent();
                return domainEvents;
            })
            .Select(domainEvent=> new OutBoxMessage(
                Guid.NewGuid(),
                _dateTimeProvider.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)))
            .ToList();
        // foreach (var domainEvent in domainEvents)
        // {
        //         await _publisher.Publish(domainEvent);
        // }

        AddRange(outBoxMessages); //adding to changetracker
    }
}