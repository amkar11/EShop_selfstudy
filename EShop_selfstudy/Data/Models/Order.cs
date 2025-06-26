using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EShop_selfstudy.Data.Models
{
    public class Order
    {
        [Key]
        [BindNever]
        public int orderId { get; set; }

        [Display(Name = "Name")]
        [StringLength(25)]
        [Required(ErrorMessage = "Name length must be not less than 2 symbols")]
        public string name { get; set; } = default!;

        [Display(Name = "Surname")]
        [StringLength(25)]
        [Required(ErrorMessage = "Name length must be not less than 3 symbols")]
        public string surname { get; set; } = default!;

        [Display(Name = "Address")]
        [StringLength(35)]
        [Required(ErrorMessage = "Address length must be not less than 15 symbols")]
        public string address { get; set; } = default!;

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(20)]
        [Required(ErrorMessage = "Phone number length must be not less than 10 symbols")]
        public string phone_number { get; set; } = default!;

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(25)]
        [Required(ErrorMessage = "Email length must be not less than 15 symbols")]
        public string email { get; set; } = default!;

        [BindNever]
        [ScaffoldColumn(false)]
        public DateTime orderTime { get; set; } = DateTime.Now;

        [BindNever]
        [ScaffoldColumn(false)]
        public int userId { get; set; } = default!;

        [BindNever]
        [ScaffoldColumn(false)]
        public string shopCartId { get; set; } = default!;

        public List<OrderDetail>? order_details { get; set; }

    }
}
