using Catalog.API.Data;
using MongoDB.Driver;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(string Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required.");
    }
}

public class DeleteProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
                .Find(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken) 
                      ?? throw new ProductNotFoundException(request.Id);
        
        await dbContext.Products.DeleteOneAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        return new DeleteProductResult(true);
    }
}