using System;
using System.Collections.Generic;

namespace BikeDistributor.DiscountCalculators
{
    /// <summary>
    /// A discount calculator that delegates discount calculations to multiple discount strategies.
    /// Since StrategyDiscountCalculator is an IDiscountCalculator, it can be added as a strategy too.
    /// However it is an error to add the StrategyDiscountCalculator to itself.
    /// </summary>
    public class DiscountStrategyCalculator<TType> : IDiscountCalculator<TType>
    {
        private readonly Func<IEnumerable<IDiscountCalculator<TType>>, TType, decimal> _discountOperation;
        private readonly IList<IDiscountCalculator<TType>> _discountStrategies = new List<IDiscountCalculator<TType>>();

        /// <summary>
        /// Creates a new StrategyDiscountCalculator using the discount operation specified.
        /// 
        /// Examples of discount operations can be:
        /// <example>
        ///     Sums the discounts as percentages of the price of a line (if more than one discount returns a value > 0, then it will be applied)
        ///     <code>
        ///         (strategies, line) => strategies.Sum(strat => strat.GetDiscount(line))
        ///     </code>
        /// </example>
        /// <example>
        ///     Takes the best discount as percentages of the price of a line (if more than one discount returns a value > 0, then only the greater one will be selected)
        ///     <code>
        ///         (strategies, line) => strategies.Max(strat => strat.GetDiscount(line))
        ///     </code>
        /// </example>
        /// </summary>
        /// <param name="discountOperation"></param>
        public DiscountStrategyCalculator(Func<IEnumerable<IDiscountCalculator<TType>>, TType, decimal> discountOperation)
        {
            _discountOperation = discountOperation;
        }

        /// <summary>
        /// Creates a default StrategyDiscountCalculator.
        /// The default calculator takes the sum of all the strategies to apply a discount.
        /// </summary>
        public DiscountStrategyCalculator()
            : this(DiscountStrategyOperations.Sum)
        {

        }

        /// <summary>
        /// Adds a new discount calculator.
        /// The calculators are stored in the order they were added, and their order should not matter.
        /// </summary>
        /// <param name="calculator">The calculator to add.</param>
        public void AddCalculator(IDiscountCalculator<TType> calculator)
        {
            if (calculator == this)
            {
                throw new ArgumentException("Cannot add the calculator as itself, as it creates an infinite loop.");
            }
            _discountStrategies.Add(calculator);
        }

        /// <summary>
        /// Calculates the discount for an object.
        /// </summary>
        /// <param name="obj">The object to apply the discount on.</param>
        /// <returns>The amount to deduct from the order.</returns>
        public decimal GetDiscount(TType obj)
        {
            return _discountOperation(_discountStrategies, obj);
        }

        /// <summary>
        /// Convenience method to build a PredicateDiscount calculator and
        /// add it to this calculator.
        /// </summary>
        /// <returns>a new PredicateDiscountBuilder.</returns>
        public PredicateDiscountBuilder<TType> Configure()
        {
            return new PredicateDiscountBuilder<TType>(this);
        }
    }
}