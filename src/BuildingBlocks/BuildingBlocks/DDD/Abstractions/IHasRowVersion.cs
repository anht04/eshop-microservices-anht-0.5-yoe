namespace BuildingBlocks.DDD.Abstractions;

public interface IHasRowVersion
{
    byte[] RowVersion { get; set; }
}