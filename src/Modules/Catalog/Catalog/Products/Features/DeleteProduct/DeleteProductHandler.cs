﻿namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid Id)
     :ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);
public class DeleteProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
          .FindAsync([command.Id], cancellationToken: cancellationToken);
        if (product is null)
            throw new Exception($"Product not found: {command.Id}");

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();

        return new DeleteProductResult(true);
    }
}
