using Catalog.API.Data;
using MongoDB.Driver;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(IEnumerable<Product> Products);

public class GetProductsByCategoryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{

    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Find(p => p.Category.Contains(request.Category))
            .ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(products);
    }
}