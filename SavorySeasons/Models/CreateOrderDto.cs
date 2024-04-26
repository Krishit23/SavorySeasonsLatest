namespace SavorySeasons.Models
{
    public class CreateOrderDto
    {
        public string DishName { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}
