namespace Catalog.Products.Features.CreateProduct;

public record CreateProductRequest(ProductDto ProductDto);
public record CreateProductResponse(Guid Id);

public class CreateProductEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();

            var result = await sender.Send(command);

            var respone = result.Adapt<CreateProductResponse>();

            return Results.Created($"/products/{respone.Id}", respone);
        })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
    }
}
