using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

public sealed record BookingCanceledDomainEvent(Guid Id):IDomainEvents;