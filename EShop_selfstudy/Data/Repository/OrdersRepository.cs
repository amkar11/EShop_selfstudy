using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.Data.Models;

namespace EShop_selfstudy.Data.Repository
{
    public class OrdersRepository : IAllOrders
    {
        private readonly IRepository _repository;
        private readonly ShopCart _shopCart;
        public OrdersRepository(IRepository repository, ShopCart cart)
        {
            _repository = repository;
            _shopCart = cart;
        }
        public void CreateOrder(Order order)
        {
            _repository.AddAsync<Order>(order);
            var items = _shopCart.listShopItems;

            foreach (var el in items)
            {
                var orderDetail = new OrderDetail
                {
                    carId = el.car.carId,
                    orderId = order.orderId,
                    price = el.car.price
                };
                _repository.AddAsync<OrderDetail>(orderDetail);
            }
            
        }
    }
}
