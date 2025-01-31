namespace Bookify.Domain.Abstractions;

public abstract class Entity(Guid id)
{
    private readonly List<IDomainEvents> _domainEvents = new()  ;
    public Guid Id { get; init; } = id;

    public IReadOnlyCollection<IDomainEvents> DomainEvents => _domainEvents.AsReadOnly();

    public List<IDomainEvents> GetDomainEvents() => _domainEvents.ToList();
    public void RaiseDomainEvents(IDomainEvents domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvent( ) => _domainEvents.Clear() ;
}