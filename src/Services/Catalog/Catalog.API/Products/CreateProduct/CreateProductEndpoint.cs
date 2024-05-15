using Carter;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    // CreateProductRequest是request的class，collect all the info for creating a new product
    public record CreateProductResponse(Guid Id);
    // CreateProductResponse是response的class 
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            throw new NotImplementedException();
        }
    }
}
