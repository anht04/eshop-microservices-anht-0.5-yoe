namespace BuildingBlocks.DDD.Abstractions;

public interface IHasModificationAudit
{
    DateTimeOffset? LastModified { get; set; }
    string? LastModifiedBy { get; set; }
}