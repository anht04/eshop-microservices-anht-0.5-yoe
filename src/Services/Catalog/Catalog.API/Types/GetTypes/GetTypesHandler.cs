using BuildingBlocks.Pagination;
using Catalog.API.Data;
using MongoDB.Driver;

namespace Catalog.API.Types.GetTypes;

public record GetTypesQuery(PaginationRequest Request) : IQuery<GetTypesResult>;

public record GetTypesResult(PaginatedResult<ProductType> Types);

public class GetTypesHandler(CatalogDbContext dbContext): IQueryHandler<GetTypesQuery, GetTypesResult>
{
    public async Task<GetTypesResult> Handle(GetTypesQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var totalCount = await dbContext.ProductTypes.CountDocumentsAsync(FilterDefinition<ProductType>.Empty, cancellationToken: cancellationToken);

        var types = await dbContext.ProductTypes
            .Find(_ => true)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Limit(request.PageSize)
            .ToListAsync(cancellationToken);

        return new GetTypesResult(new PaginatedResult<ProductType>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Data = types
        });
    }
}