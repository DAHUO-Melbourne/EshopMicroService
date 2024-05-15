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
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                // 使用Mapster的Adapt方法，将request转化为类型为CreateProductCommand的command
                var result = await sender.Send(command);
                // 使用MediatR的sender的send方法，将command转发出去。MediatR会根据command的类型自动定位到对应的handler里
                var response = result.Adapt<CreateProductResponse>();
                // 将response再转换回去
                return Results.Created($"/products/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
            // 这些相当于给这个endpoint加一些附加的信息，比如说两个produce就相当于传统endpoint里可能的status code
        }
    }
}
