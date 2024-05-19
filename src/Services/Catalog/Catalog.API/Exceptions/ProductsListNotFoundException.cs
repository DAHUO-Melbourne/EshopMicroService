namespace Catalog.API.Exceptions
{
    public class ProductsListNotFoundException: Exception
    {
        public ProductsListNotFoundException(): base("Product not Found!")
        {
            
        }
    }
}
