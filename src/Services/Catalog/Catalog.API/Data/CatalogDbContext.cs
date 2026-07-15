using Catalog.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogDbContext
{
    public IMongoCollection<Product> Products { get; }
    public IMongoCollection<ProductBrand> ProductBrands { get; }
    public IMongoCollection<ProductType> ProductTypes { get; }

    public CatalogDbContext(IOptions<DatabaseSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        Products = database.GetCollection<Product>(settings.ProductCollectionName);
        ProductBrands = database.GetCollection<ProductBrand>(settings.BrandCollectionName);
        ProductTypes = database.GetCollection<ProductType>(settings.TypeCollectionName);
    }
}