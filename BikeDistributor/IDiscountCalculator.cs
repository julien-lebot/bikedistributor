using BikeDistributor.Models;

namespace BikeDistributor
{
    /// <summary>
    /// A discount calculator is used to apply various deductions
    /// on an order. It can apply to the whole order as well as individual lines.
    /// </summary>
    public interface IDiscountCalculator : ILineDiscountStrategy, IOrderDiscountStrategy
    {
    }
}