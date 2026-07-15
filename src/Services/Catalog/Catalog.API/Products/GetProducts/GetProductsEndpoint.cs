using BuildingBlocks.Pagination;

namespace Catalog.API.Products.GetProducts;

public record GetProductsRequest(Guid? BrandId, Guid? TypeId, string? Sort, string? Search, int PageIndex = 1, int PageSize = 10);

public record GetProductsResponse(PaginatedResult<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>() with
                {
                    Pagination = new PaginationRequest(request.PageIndex, request.PageSize)
                };

                var result = await sender.Send(query);

                var response = result.Adapt<GetProductsResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get Products")
            .WithSummary("Get Products");
    }
}
