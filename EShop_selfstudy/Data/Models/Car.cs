using System.ComponentModel.DataAnnotations;

namespace EShop_selfstudy.Data.Models
{
    public class Car
    {
        [Key]
        public int carId { get; set; }
        public string name { get; set; } = default!;
        public string shortDesc { get; set; } = default!;
        public string longDesc { get; set; } = default!;
        public string img { get; set; } = default!;
        public ushort price { get; set; }
        public bool isFavourite { get; set; }
        public bool available { get; set; }
        public int categoryId { get; set; }
        public virtual Category? category { get; set; }
    }
}
