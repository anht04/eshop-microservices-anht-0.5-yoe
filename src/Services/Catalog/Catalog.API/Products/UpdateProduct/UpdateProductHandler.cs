using Catalog.API.Data;
using MongoDB.Driver;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(
    string Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .Length(2, 150)
            .WithMessage("Name must be between 2 and 150 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }
}

public class UpdateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products 
            .Find(p => p.Id == command.Id) 
            .FirstOrDefaultAsync(cancellationToken) 
                      ?? throw new ProductNotFoundException(command.Id);

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        await dbContext.Products.ReplaceOneAsync(p => p.Id == command.Id, product, cancellationToken: cancellationToken);

        return new UpdateProductResult(true);
    }
}