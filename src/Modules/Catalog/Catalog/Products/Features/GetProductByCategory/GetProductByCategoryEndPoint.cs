using Catalog.Products.Features.GetProductById;

namespace Catalog.Products.Features.GetProductByCategory;

public record GetProductByCategoryResponse(ProductDto Product);
public class GetProductByCategoryEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{category}", async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByCategoryQuery(category));

            var response = result.Adapt<GetProductByCategoryResponse>();

            return Results.Ok(response);
        })
            .WithName("GetProductByCategory")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Category")
            .WithDescription("Get Product By Category");
    }
}
