using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid id) : base(nameof(Product), id)
    {
        
    }    
    
    public ProductNotFoundException(string id) : base(nameof(Product), id)
    {
        
    }
}