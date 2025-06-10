using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop_selfstudy.Data.Models
{
    public class ShopCartItem
    {
        [Key]
        public int shopCartItemId { get; set; }

        public int carId { get; set; } = default!;
        public int price { get; set; } = default!;
        public string shopCartId { get; set; } = default!;
        public int? orderId { get; set; }

        [ForeignKey("orderId")]
        public Order? order { get; set; } = default!;

        [ForeignKey("carId")]
        public Car car { get; set; } = default!;


    }
}
