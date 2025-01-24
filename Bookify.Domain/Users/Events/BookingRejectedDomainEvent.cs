using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

public sealed record BookingRejectedDomainEvent(Guid Id):IDomainEvents;