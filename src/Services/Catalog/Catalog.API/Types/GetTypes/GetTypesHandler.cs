using BuildingBlocks.Pagination;
using Catalog.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Types.GetTypes;

public record GetTypesQuery(PaginationRequest Request) : IQuery<GetTypesResult>;

public record GetTypesResult(PaginatedResult<ProductType> Types);

public class GetTypesHandler(CatalogDbContext dbContext): IQueryHandler<GetTypesQuery, GetTypesResult>
{
    public async Task<GetTypesResult> Handle(GetTypesQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var totalCount = await dbContext.ProductTypes.CountAsync(cancellationToken: cancellationToken);

        var Types = await dbContext.ProductTypes
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new GetTypesResult(new PaginatedResult<ProductType>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Data = Types
        });
    }
}