using BuildingBlocks.DDD;
using MongoDB.Driver;

namespace Catalog.API.Data;

public static class CatalogDbContextSeed
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        await SeedAsync(dbContext);
    }

    private static async Task SeedAsync(CatalogDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var brandCount = await dbContext.ProductBrands.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken);
        if (brandCount == 0)
        {
            await dbContext.ProductBrands.InsertManyAsync(GetPreconfiguredBrands(), cancellationToken: cancellationToken);
        }

        var typeCount = await dbContext.ProductTypes.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken);
        if (typeCount == 0)
        {
            await dbContext.ProductTypes.InsertManyAsync(GetPreconfiguredTypes(), cancellationToken: cancellationToken);
        }

        var productCount = await dbContext.Products.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken);
        if (productCount == 0)
        {
            var brands = await dbContext.ProductBrands.Find(_ => true).ToListAsync(cancellationToken);
            var types = await dbContext.ProductTypes.Find(_ => true).ToListAsync(cancellationToken);

            var products = GetPreconfiguredProducts(brands, types);
            await dbContext.Products.InsertManyAsync(products, cancellationToken: cancellationToken);
        }
    }

    private static IEnumerable<ProductBrand> GetPreconfiguredBrands()
    {
        return
        [
            new ProductBrand { Id = "111111111111111111111111", Name = "Apple" },
            new ProductBrand { Id = "222222222222222222222222", Name = "Samsung" },
            new ProductBrand { Id = "333333333333333333333333", Name = "Huawei" },
            new ProductBrand { Id = "444444444444444444444444", Name = "Xiaomi" },
            new ProductBrand { Id = "555555555555555555555555", Name = "HTC" },
            new ProductBrand { Id = "666666666666666666666666", Name = "LG" },
            new ProductBrand { Id = "777777777777777777777777", Name = "Panasonic" }
        ];
    }

    private static IEnumerable<ProductType> GetPreconfiguredTypes()
    {
        return
        [
            new ProductType { Id = "aaaaaaaaaaaaaaaaaaaaaaaa", Name = "Smart Phone" },
            new ProductType { Id = "bbbbbbbbbbbbbbbbbbbbbbbb", Name = "White Appliances" },
            new ProductType { Id = "cccccccccccccccccccccccc", Name = "Home Kitchen" },
            new ProductType { Id = "dddddddddddddddddddddddd", Name = "Camera" }
        ];
    }

    private static IEnumerable<Product> GetPreconfiguredProducts(List<ProductBrand> brands, List<ProductType> types)
    {
        var products = new List<Product>
        {
            new Product
            {
                Id = "5334c99684574cf0815ced2b",
                Name = "IPhone X",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-1.png",
                Price = 950.00M,
                Category = ["Smart Phone"],
                Brand = GetBrand("Apple"),
                Type = GetType("Smart Phone")
            },
            new Product
            {
                Id = "c67d6323e8b14bdf9a75b0d0",
                Name = "Samsung 10",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-2.png",
                Price = 840.00M,
                Category = ["Smart Phone"],
                Brand = GetBrand("Samsung"),
                Type = GetType("Smart Phone")
            },
            new Product
            {
                Id = "4f136e9fff8c4c1f9a33d12f",
                Name = "Huawei Plus",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-3.png",
                Price = 650.00M,
                Category = ["White Appliances"],
                Brand = GetBrand("Huawei"),
                Type = GetType("White Appliances")
            },
            new Product
            {
                Id = "6ec1297bec0a4aa1be256726",
                Name = "Xiaomi Mi 9",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-4.png",
                Price = 470.00M,
                Category = ["White Appliances"],
                Brand = GetBrand("Xiaomi"),
                Type = GetType("White Appliances")
            },
            new Product
            {
                Id = "b786103dc6214f5ab4982345",
                Name = "HTC U11+ Plus",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-5.png",
                Price = 380.00M,
                Category = ["Smart Phone"],
                Brand = GetBrand("HTC"),
                Type = GetType("Smart Phone")
            },
            new Product
            {
                Id = "c4bbc4a2455545d897cc2a99",
                Name = "LG G7 ThinQ",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-6.png",
                Price = 240.00M,
                Category = ["Home Kitchen"],
                Brand = GetBrand("LG"),
                Type = GetType("Home Kitchen")
            },
            new Product
            {
                Id = "93170c857795489c8e8f7dcf",
                Name = "Panasonic Lumix",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-6.png",
                Price = 240.00M,
                Category = ["Camera"],
                Brand = GetBrand("Panasonic"),
                Type = GetType("Camera")
            }
        };

        foreach (var product in products)
        {
            // Đã đổi từ AuditableEntity<Guid> sang AuditableEntity<string>
            if (product is AuditableEntity<string> auditable && auditable.CreatedAt == default)
            {
                auditable.CreatedAt = DateTime.UtcNow;
            }
        }

        return products;

        ProductType GetType(string name) => types.Single(t => t.Name == name);
        ProductBrand GetBrand(string name) => brands.Single(b => b.Name == name);
    }
}