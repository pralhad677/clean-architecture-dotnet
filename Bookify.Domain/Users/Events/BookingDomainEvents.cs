using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

public sealed  record BookingDomainEvents(Guid Id):IDomainEvents;