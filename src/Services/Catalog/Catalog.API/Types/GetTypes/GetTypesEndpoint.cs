using BuildingBlocks.Pagination;

namespace Catalog.API.Types.GetTypes;

public record GetTypesResponse(PaginatedResult<ProductType> Types);

public class GetTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/Types", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetTypesQuery(request));

                var response = result.Adapt<GetTypesResponse>();

                return Results.Ok(response);
            })
            .WithName("GetTypes")
            .Produces<GetTypesResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get Types")
            .WithSummary("Get Types");
    }
}
