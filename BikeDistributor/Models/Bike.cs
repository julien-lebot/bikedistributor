
namespace BikeDistributor.Models
{
    public class Bike
    {
        public Bike(string brand, string model, decimal price)
        {
            Brand = brand;
            Model = model;
            Price = price;
        }

        public string Brand
        {
            get;
            set;
        }

        public string Model
        {
            get;
            set;
        }

        public decimal Price
        {
            get;
            set;
        }
    }
}
