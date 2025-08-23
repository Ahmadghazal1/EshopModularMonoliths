
namespace Catalog.Contracts.Products.Features.GetProdcutById;
public record GetProductByIdQuery(Guid Id)
    : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(ProductDto Product);