using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Data;

public static class CatalogDbContextSeed
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        await SeedAsync(context);
    }

    public static async Task SeedAsync(CatalogDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (!await dbContext.ProductBrands.AnyAsync(cancellationToken))
        {
            await dbContext.ProductBrands.AddRangeAsync(GetPreconfiguredBrands(), cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        if (!await dbContext.ProductTypes.AnyAsync(cancellationToken))
        {
            await dbContext.ProductTypes.AddRangeAsync(GetPreconfiguredTypes(), cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        if (!await dbContext.Products.AnyAsync(cancellationToken))
        {
            var brands = await dbContext.ProductBrands.ToListAsync(cancellationToken);
            var types = await dbContext.ProductTypes.ToListAsync(cancellationToken);

            await dbContext.Products.AddRangeAsync(GetPreconfiguredProducts(brands, types), cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static IEnumerable<ProductBrand> GetPreconfiguredBrands()
    {
        return
        [
            new ProductBrand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Apple" },
            new ProductBrand { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Samsung" },
            new ProductBrand { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Huawei" },
            new ProductBrand { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Xiaomi" },
            new ProductBrand { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "HTC" },
            new ProductBrand { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "LG" },
            new ProductBrand { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Panasonic" }
        ];
    }

    private static IEnumerable<ProductType> GetPreconfiguredTypes()
    {
        return
        [
            new ProductType { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Smart Phone" },
            new ProductType { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "White Appliances" },
            new ProductType { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Home Kitchen" },
            new ProductType { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "Camera" }
        ];
    }

    private static IEnumerable<Product> GetPreconfiguredProducts(List<ProductBrand> brands, List<ProductType> types)
    {
        return
        [
            new Product
            {
                Id = Guid.Parse("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
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
                Id = Guid.Parse("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
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
                Id = Guid.Parse("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
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
                Id = Guid.Parse("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
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
                Id = Guid.Parse("b786103d-c621-4f5a-b498-23452610f88c"),
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
                Id = Guid.Parse("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
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
                Id = Guid.Parse("93170c85-7795-489c-8e8f-7dcf3b4f4188"),
                Name = "Panasonic Lumix",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-6.png",
                Price = 240.00M,
                Category = ["Camera"],
                Brand = GetBrand("Panasonic"),
                Type = GetType("Camera")
            }
        ];

        ProductType GetType(string name) => types.Single(t => t.Name == name);

        ProductBrand GetBrand(string name) => brands.Single(b => b.Name == name);
    }
}