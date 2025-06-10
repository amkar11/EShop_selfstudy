using System.ComponentModel.DataAnnotations;

namespace EShop_selfstudy.Data.Models
{
    public class OrderDetail
    {
        [Key]
        public int orderDetailId { get; set; }
        public int orderId { get; set; }
        public int carId { get; set; }
        public uint price { get; set; }
        public virtual Car car { get; set; } = default!;
        public virtual Order order { get; set; } = default!;
    }
}
