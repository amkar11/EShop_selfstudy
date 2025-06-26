using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICartCreater
    {
        Task CreateCartAsync(int userId);
    }
}
