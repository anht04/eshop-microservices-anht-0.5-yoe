using BuildingBlocks.Pagination;
using Catalog.API.Data;
using MongoDB.Driver;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(PaginationRequest Pagination, string? BrandId, string? TypeId, string? Sort, string? Search)
    : IQuery<GetProductsResult>;

public record GetProductsResult(PaginatedResult<Product> Products);

public class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var pagination = query.Pagination;

        var filterDefinition = ApplyDataFilters(query);
        var sortDefinition = ApplySorting(query);

        var products = await dbContext.Products
            .Find(filterDefinition)
            .Sort(sortDefinition)
            .Skip((pagination.PageIndex - 1) * pagination.PageSize)
            .Limit(pagination.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await dbContext.Products.CountDocumentsAsync(
            filterDefinition, 
            cancellationToken: cancellationToken);
        
        return new GetProductsResult(new PaginatedResult<Product>
        {
            PageIndex = pagination.PageIndex,
            PageSize = pagination.PageSize,
            TotalCount = totalCount,
            Data = products
        });
    }

    private static FilterDefinition<Product> ApplyDataFilters(GetProductsQuery query)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            filter &= builder.Where(p => p.Name.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
        }
        if (!string.IsNullOrWhiteSpace(query.BrandId))
        {
            filter &= builder.Eq(p => p.Brand.Id, query.BrandId);
        }
        if (!string.IsNullOrWhiteSpace(query.TypeId))
        {
            filter &= builder.Eq(p => p.Type.Id, query.TypeId);
        }

        return filter;
    }
    
    private static SortDefinition<Product> ApplySorting(GetProductsQuery query)
    {
        var sortDefinition = Builders<Product>.Sort.Ascending(p => p.Name);
        if (!string.IsNullOrWhiteSpace(query.Sort))
        {
            sortDefinition = query.Sort switch
            {
                "priceAsc" => Builders<Product>.Sort.Ascending(p => p.Price),
                "priceDesc" => Builders<Product>.Sort.Descending(p => p.Price),
                _ => Builders<Product>.Sort.Ascending(p => p.Name)
            };
        }

        return sortDefinition;
    }
}