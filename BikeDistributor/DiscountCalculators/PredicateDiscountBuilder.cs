using System;

namespace BikeDistributor.DiscountCalculators
{
    /// <summary>
    /// Helper class to build a PredicateDiscount using a "Fluent" interface.
    /// </summary>
    /// <typeparam name="TType">The object to use for the discount calculation.</typeparam>
    public class PredicateDiscountBuilder<TType>
    {
        private PredicateDiscountCalculator<TType> _predicateDiscountCalculator = null;
        private readonly DiscountStrategyCalculator<TType> _discountStrategyCalculator;

        public PredicateDiscountBuilder(DiscountStrategyCalculator<TType> calculator)
        {
            _discountStrategyCalculator = calculator;
        }

        public PredicateDiscountBuilder<TType> When(Predicate<TType> predicate)
        {
            if (_predicateDiscountCalculator == null)
            {
                _predicateDiscountCalculator = new PredicateDiscountCalculator<TType>();
            }
            _predicateDiscountCalculator.Predicate = predicate;
            return this;
        }

        public PredicateDiscountBuilder<TType> ApplyDiscount(Func<TType, decimal> discountFunc)
        {
            _predicateDiscountCalculator.DiscountCalculator = discountFunc;
            _discountStrategyCalculator.AddCalculator(_predicateDiscountCalculator);
            _predicateDiscountCalculator = null;
            return this;
        }
    }
}