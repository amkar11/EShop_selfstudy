using EShop_selfstudy.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EShop_selfstudy.Data.Models
{
    public class ShopCart
    {
        public string ShopCartId { get; set; }

        private readonly IRepository _repository;
        public IEnumerable<ShopCartItem> listShopItems { get; set; } = Enumerable.Empty<ShopCartItem>();
        public ILogger<ShopCart> _logger { get; set; }

        public ShopCart(IRepository repository, ILogger<ShopCart> logger)
        {
            _repository = repository;
            ShopCartId = string.Empty;
            _logger = logger;
          
        }


        public static ShopCart GetCart(IServiceProvider services)
        {
            HttpContext? httpcontext = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
            if (httpcontext == null) throw new InvalidOperationException("HttpContext is not accessible now");
            ISession session = httpcontext.Session;
            if (session == null) throw new InvalidOperationException("Session is not accessible now");
            var repository = services.GetRequiredService<IRepository>();
            var logger = services.GetRequiredService<ILogger<ShopCart>>();

            string shopCartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();  

            session.SetString("CartId", shopCartId);

            return new ShopCart(repository, logger) { ShopCartId = shopCartId };
        }

        public async Task AddToCartAsync(Car car)
        {
            await _repository.AddAsync<ShopCartItem>(new ShopCartItem
            {
                shopCartId = ShopCartId,
                carId = car.carId,
                price = car.price
            });
        }

        public  async Task DeleteAllItemsFromCartAsync(string shopCartId)
        {
            var items = await getShopItemsAsync(shopCartId);
            try
            {
                foreach (var item in items)
                {
                    await _repository.DeleteAsync<ShopCartItem>(item.shopCartItemId);
                }
                _logger.LogInformation("All the items from current shop cart were successfully removed: {shopCart}", getShopItemsAsync(shopCartId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while deleting items from current cart!: {error}", ex.Message);
            }
        }

        public async Task<IEnumerable<ShopCartItem>> getShopItemsAsync(string ShopCartId)
        {
            return await _repository.GetAll<ShopCartItem>(s => s.car).Where(x => x.shopCartId == ShopCartId).ToListAsync();
        }
        
    }
}
