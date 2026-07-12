namespace BuildingBlocks.DDD.Abstractions;

public interface IEntity<T> : IEntity
{
    public T Id { get; init; }
}

public interface IEntity;