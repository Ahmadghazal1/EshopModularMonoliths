namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery 
       :IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<ProductDto> Products);
public class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
                   .AsNoTracking()
                   .OrderBy(x => x.Name)
                   .ToListAsync(cancellationToken);
        var productsDtos = products.Adapt<List<ProductDto>>();

        return new GetProductsResult(productsDtos);
    }
}
