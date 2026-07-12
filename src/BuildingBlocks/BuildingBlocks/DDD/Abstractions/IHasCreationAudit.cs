namespace BuildingBlocks.DDD.Abstractions;

public interface IHasCreationAudit
{
    DateTimeOffset CreatedAt { get; set; }
    string? CreatedBy { get; set; }
}