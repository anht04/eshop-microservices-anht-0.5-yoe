using BuildingBlocks.Pagination;
using Catalog.API.Products.GetProducts;

namespace Catalog.API.Brands.GetBrands;

public record GetBrandsResponse(PaginatedResult<ProductBrand> Brands);

public class GetBrandsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/brands", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetBrandsQuery(request));

                var response = result.Adapt<GetBrandsResponse>();

                return Results.Ok(response);
            })
            .WithName("GetBrands")
            .Produces<GetBrandsResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get Brands")
            .WithSummary("Get Brands");
    }
}
