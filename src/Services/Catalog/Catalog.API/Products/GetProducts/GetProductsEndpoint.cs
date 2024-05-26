
namespace Catalog.API.Products.GetProducts
{
    public record GetProductRequest(int? PageNumber = 1, int? PageSize = 10);
    public record GetProductResponse(IEnumerable<Product> Products);
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductRequest request, ISender sender) =>
            // AsParameters意思是从parameter里拿参数
            {
                var query = request.Adapt<GetProductsQuery>();
                // 使用类似mapping的方法将request mapping到query上，并发送出去
                // 也就是：/products?pageNumber=1&pageSize=5
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
        }
    }
}
