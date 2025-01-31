using Bookify.Application.Exception;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure;

public sealed  class ApplicationDbContext:DbContext,IUnitOfWork
{
    private readonly IPublisher _publisher;
    public ApplicationDbContext(DbContextOptions options, IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
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


            var result = await base.SaveChangesAsync(cancellationToken);
            await PublishDomainEventAsync();
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concureency Exception Occurs",ex);
        }
    }

    private async Task PublishDomainEventAsync()
    {
        var domainEvents = this.ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvent();
                return domainEvents;
            })
            .ToList();
        foreach (var domainEvent in domainEvents)
        {
                await _publisher.Publish(domainEvent);
        }
    }
}