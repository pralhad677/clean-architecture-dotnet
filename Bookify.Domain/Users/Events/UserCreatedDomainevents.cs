using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

public sealed record UserCreatedDomainevents(Guid userId) :IDomainEvents;