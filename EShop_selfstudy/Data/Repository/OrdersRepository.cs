using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.Data.Models;

namespace EShop_selfstudy.Data.Repository
{
    public class OrdersRepository : IAllOrders
    {
        private readonly IRepository _repository;
        public OrdersRepository(IRepository repository)
        {
            _repository = repository;
        }
        public void CreateOrder(Order order)
        {
            _repository.AddAsync<Order>(order);

           
            }
           
        }
    }
