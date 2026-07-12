using BuildingBlocks.DDD;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Models;

public class ProductType : Entity<Guid>
{
    [BsonElement("Name")]
    public string Name { get; set; }
}