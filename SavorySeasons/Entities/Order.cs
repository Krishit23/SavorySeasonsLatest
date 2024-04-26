using SavorySeasons.Models;

namespace SavorySeasons.Entities
{
    public class Order
    {

        public const string OrderTable = "Orders";

        public int Id { get; set; }

        public string DishName { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public string userId { get; set; }

        public ApplicationUser ApplicationUser { get; set; } 
    }
}
