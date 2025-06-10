using System.ComponentModel.DataAnnotations;

namespace EShop_selfstudy.Data.Models
{
    public class Category
    {
        [Key]
        public int categoryId { get; set; }
        public string categoryName { get; set; } = default!;
        public string desc { get; set; } = default!;
        public List<Car>? Cars { get; set; }
    }
}
