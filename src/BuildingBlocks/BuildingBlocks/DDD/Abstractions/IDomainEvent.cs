using MediatR;

namespace BuildingBlocks.DDD.Abstractions;

public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();
    
    public DateTimeOffset OccurredOn => DateTimeOffset.Now;
    
    public string EventType => GetType().AssemblyQualifiedName!;
}