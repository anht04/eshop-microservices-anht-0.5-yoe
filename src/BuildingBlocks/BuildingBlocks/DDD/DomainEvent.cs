using BuildingBlocks.DDD.Abstractions;

namespace BuildingBlocks.DDD;

public abstract record DomainEvent(Guid EventId) : IDomainEvent
{
    protected DomainEvent() : this(Guid.NewGuid()) { }

    public DateTimeOffset OccurredOn => DateTimeOffset.Now;
    
    public string EventType => GetType().AssemblyQualifiedName!;
}
