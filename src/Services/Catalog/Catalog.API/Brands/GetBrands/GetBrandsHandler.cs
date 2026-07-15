using BuildingBlocks.Pagination;
using Catalog.API.Data;
using MongoDB.Driver;

namespace Catalog.API.Brands.GetBrands;

public record GetBrandsQuery(PaginationRequest Request) : IQuery<GetBrandsResult>;

public record GetBrandsResult(PaginatedResult<ProductBrand> Brands);

public class GetBrandsHandler(CatalogDbContext dbContext): IQueryHandler<GetBrandsQuery, GetBrandsResult>
{
    public async Task<GetBrandsResult> Handle(GetBrandsQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var totalCount = await dbContext.ProductBrands.CountDocumentsAsync(FilterDefinition<ProductBrand>.Empty, cancellationToken: cancellationToken);

        var brands = await dbContext.ProductBrands
            .Find(_ => true)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Limit(request.PageSize)
            .ToListAsync(cancellationToken);

        return new GetBrandsResult(new PaginatedResult<ProductBrand>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Data = brands
        });
    }
}