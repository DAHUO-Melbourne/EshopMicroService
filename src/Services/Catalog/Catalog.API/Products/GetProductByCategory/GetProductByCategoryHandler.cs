namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    internal class GetProductByCategoryQueryHandler
        (IDocumentSession session, ILogger<GetProductByCategoryResult> logger)
            : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByCategoryHandler.Handler called with {@query}", query);

            var products = await session.Query<Product>().Where(p => p.Category.Contains(query.Category)).ToListAsync();

            if (products == null)
            {
                throw new ProductsListNotFoundException();
            }

            return new GetProductByCategoryResult(products);
        }
    }
}
