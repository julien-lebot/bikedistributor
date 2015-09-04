using System.Collections.Generic;
using System.Linq;

namespace BikeDistributor.DiscountCalculators
{
    /// <summary>
    /// Regroups some common discount operations such as Sum, Max and Min.
    /// </summary>
    public static class DiscountStrategyOperations
    {
        /// <summary>
        /// Default operation for the strategies, which sums all the discounts.
        /// </summary>
        /// <param name="strategies">The discount strategies to use for discount calculation.</param>
        /// <param name="obj">The objec to apply the discount on.</param>
        /// <returns>The value to deduct from the order.</returns>
        public static decimal Sum<TType>(IEnumerable<IDiscountCalculator<TType>> strategies, TType obj)
        {
            return strategies.Sum(strat => strat.GetDiscount(obj));
        }

        /// <summary>
        /// Operation that takes the maximum discount to apply to an order.
        /// </summary>
        /// <param name="strategies">The discount strategies to use for discount calculation.</param>
        /// <param name="obj">The objec to apply the discount on.</param>
        /// <returns>The value to deduct from the order.</returns>
        public static decimal Max<TType>(IEnumerable<IDiscountCalculator<TType>> strategies, TType obj)
        {
            return strategies.Max(strat => strat.GetDiscount(obj));
        }

        /// <summary>
        /// Operation that takes the minimum discount to apply to an order.
        /// </summary>
        /// <param name="strategies">The discount strategies to use for discount calculation.</param>
        /// <param name="obj">The objec to apply the discount on.</param>
        /// <returns>The value to deduct from the order.</returns>
        public static decimal Min<TType>(IEnumerable<IDiscountCalculator<TType>> strategies, TType obj)
        {
            return strategies.Min(strat => strat.GetDiscount(obj));
        }
    }
}