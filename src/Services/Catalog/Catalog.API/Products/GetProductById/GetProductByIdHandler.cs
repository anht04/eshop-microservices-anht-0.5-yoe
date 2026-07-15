using Catalog.API.Data;
using MongoDB.Driver;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(string Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

public class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
                .Find(p => p.Id == query.Id)
                .FirstOrDefaultAsync(cancellationToken)
                      ?? throw new ProductNotFoundException(query.Id);

        return new GetProductByIdResult(product);
    }
}