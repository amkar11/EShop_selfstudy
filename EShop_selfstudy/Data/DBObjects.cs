using EShop_selfstudy.Data.Models;

namespace EShop_selfstudy.Data
{
    public class DBObjects
    {
        public static void Initial(AppDBContext context)
        {

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(Categories.Select(c => c.Value));
                context.SaveChanges();
            }

            if (!context.Car.Any())
            {
                context.Car.AddRange(
                    new Car
                    {
                        name = "Tesla Model S",
                        shortDesc = "Fast auto",
                        longDesc = "Fast, beautifull and very silent Tesla auto",
                        img = "/img/tesla.jpg",
                        price = 45000,
                        isFavourite = true,
                        available = true,
                        category = Categories["Electrocars"]
                    },
                new Car
                {
                    name = "Ford Fiesta",
                    shortDesc = "Calm and silent",
                    longDesc = "Comfortable auto for city life",
                    img = "/img/fiesta.jpg",
                    price = 11000,
                    isFavourite = false,
                    available = true,
                    category = Categories["Classic cars"]
                },
                new Car
                {
                    name = "BMW M3",
                    shortDesc = "Daring and stylish",
                    longDesc = "Comfortable auto for city life",
                    img = "/img/bmw.jpg",
                    price = 65000,
                    isFavourite = true,
                    available = true,
                    category = Categories["Classic cars"]
                },
                new Car
                {
                    name = "Mercedes S class",
                    shortDesc = "Big and cozy",
                    longDesc = "Comfortable auto for city life",
                    img = "/img/mercedes.jpg",
                    price = 40000,
                    isFavourite = false,
                    available = false,
                    category = Categories["Classic cars"]
                },
                new Car
                {
                    name = "Nissan Leaf",
                    shortDesc = "Silent and economical",
                    longDesc = "Comfortable auto for city life",
                    img = "/img/nissan.jpg",
                    price = 14000,
                    isFavourite = true,
                    available = true,
                    category = Categories["Electrocars"]
                }
                );
            }
            context.SaveChanges();
        }

        private static Dictionary<string, Category>? category;
        public static Dictionary<string, Category> Categories {
            get
            {
                if (category == null)
                {
                    var list = new Category[]
                    {
                        new Category {categoryName = "Electrocars", desc = "Modern vehicle type"},
                        new Category {categoryName = "Classic cars", desc = "Cars with internal combustion engines"}
                    };

                    category = new Dictionary<string, Category>();
                    foreach (Category c in list)
                    {
                        category.Add(c.categoryName, c);
                    }
                    
                }
                return category;
            } 
        }
    }
}
