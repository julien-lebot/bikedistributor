namespace BikeDistributor.Models
{
    /// <summary>
    /// A line in an order.
    /// An order can contain many lines, each line describing a model of bike and the quantity to purchase.
    /// </summary>
    public class Line
    {
        public Line(Bike bike, int quantity)
        {
            Bike = bike;
            Quantity = quantity;
        }

        public Bike Bike { get; private set; }
        public int Quantity { get; private set; }
    }
}
