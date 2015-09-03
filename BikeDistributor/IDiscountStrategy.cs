namespace BikeDistributor
{
    public interface IDiscountStrategy
    {
        decimal GetDiscount(Line line);
    }
}