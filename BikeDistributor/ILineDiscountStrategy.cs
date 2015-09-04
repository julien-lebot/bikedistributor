using BikeDistributor.Models;

namespace BikeDistributor
{
    public interface ILineDiscountStrategy
    {
        /// <summary>
        /// Calculates a discount for a specific item line in the order.
        /// </summary>
        /// <param name="line">The line to compute the discount for.</param>
        /// <returns>The amount to deduce from the order.</returns>
        decimal GetDiscount(Line line);
    }

    public interface IOrderDiscountStrategy
    {
        /// <summary>
        /// Calculates a discount for a whole order.
        /// </summary>
        /// <param name="order">The order to calculate the discount for.</param>
        /// <returns>The amount to deduce from the order.</returns>
        decimal GetDiscount(Order order);
    }
}