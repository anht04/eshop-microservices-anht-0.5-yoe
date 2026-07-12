using BuildingBlocks.DDD;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Models;

public class Product : AuditableEntity<Guid>
{
    public string Name { get; set; } = default!;
    
    public List<string> Category { get; set; } = [];
    
    public string Description { get; set; } = default!;
    
    public string ImageFile { get; set; } = default!;

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }

    public ProductBrand Brand { get; set; }
    
    public ProductType Type { get; set; }
}