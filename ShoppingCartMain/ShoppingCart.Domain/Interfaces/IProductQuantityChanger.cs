using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IProductQuantityChanger
    {
        Task AddProductQuantityAsync(int cartId, int itemId);
        Task SubstractProductQuantityAsync(int cartId, int itemId);
    }
}
