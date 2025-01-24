using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

public  sealed record BookingConfirmedDomainEvent(Guid Id):IDomainEvents;