using BuildingBlocks.DDD;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Models;

public class ProductBrand : Entity<string>
{
    [BsonElement("Name")]
    public string Name { get; set; }
}