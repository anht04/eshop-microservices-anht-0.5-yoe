using Catalog.API.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Catalog.API.Data;

public class CatalogDbContext : DbContext
{
    private readonly IOptions<DatabaseSettings> _dbOptions;
    public DbSet<Product> Products { get; init; } = default!;
    public DbSet<ProductBrand> ProductBrands { get; init; } = default!;
    public DbSet<ProductType> ProductTypes { get; init; } = default!;

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options, IOptions<DatabaseSettings> dbOptions) : base(options)
    {
        _dbOptions = dbOptions;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var optionValue = _dbOptions.Value; 
        modelBuilder.Entity<Product>().ToCollection(optionValue.ProductCollectionName);
        modelBuilder.Entity<ProductBrand>().ToCollection(optionValue.BrandCollectionName);
        modelBuilder.Entity<ProductType>().ToCollection(optionValue.TypeCollectionName);
    }
}