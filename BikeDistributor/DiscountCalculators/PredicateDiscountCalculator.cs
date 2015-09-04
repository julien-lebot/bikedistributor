using System;

namespace BikeDistributor.DiscountCalculators
{
    /// <summary>
    /// A generic discount calculator that uses a predicate to decide if a discount
    /// can be applied, and a function to apply the discount.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public class PredicateDiscountCalculator<TType> : IDiscountCalculator<TType>
    {
        /// <summary>
        /// The predicates dictates if the discount is applied or not.
        /// If it is null, then it is equivalent to (obj) => true;
        /// </summary>
        public Predicate<TType> Predicate
        {
            get;
            set;
        }

        private Func<TType, decimal> _discountCalculator;
        /// <summary>
        /// The function to apply to calculate the discount.
        /// Cannot be null.
        /// <exception cref="ArgumentNullException">Thrown if null.</exception>
        /// </summary>
        public Func<TType, decimal> DiscountCalculator
        {
            get
            {
                return _discountCalculator;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _discountCalculator = value;
            }
        }

        /// <summary>
        /// Calculates the discount using the predicate and the calculation function.
        /// </summary>
        /// <param name="obj">The objec to use for discount calculation.</param>
        /// <exception cref="InvalidOperationException">thrown if the DiscountCalculator property is null.</exception>
        /// <returns>The absolute amount to deduct from the order. If the predicate returns false, then returns 0.</returns>
        public decimal GetDiscount(TType obj)
        {
            if (DiscountCalculator == null)
            {
                throw new InvalidOperationException("DiscountCalculator cannot be null");
            }
            if ((Predicate != null && Predicate(obj)) || Predicate == null)
            {
                return DiscountCalculator(obj);
            }
            return 0;
        }
    }
}