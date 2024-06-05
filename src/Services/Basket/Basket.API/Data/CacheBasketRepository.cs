
namespace Basket.API.Data
{
    public class CacheBasketRepository(IBasketRepository repository) : IBasketRepository
    // 继承IBasketRepository，但是实现方法是和cache进行交互，而不是像BasketRepository那样和数据库进行交互
    {
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            return await repository.GetBasket(userName, cancellationToken);
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            return await repository.StoreBasket(basket, cancellationToken);
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            return await repository.DeleteBasket(userName, cancellationToken);
        }
    }
}
