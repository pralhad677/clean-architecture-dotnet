using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

public sealed record BookingCompletedDomainEvent(Guid Id):IDomainEvents;