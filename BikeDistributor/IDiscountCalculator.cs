namespace BikeDistributor
{
    /// <summary>
    /// Calculates a discount given an object; the object can be the order itself, or a sub-element of the order (such as a Line).
    /// </summary>
    /// <typeparam name="TType">The object used to base the discount calculation on.</typeparam>
    public interface IDiscountCalculator<in TType>
    {
        /// <summary>
        /// Calculates a discount for an order.
        /// </summary>
        /// <param name="obj">The object to use to calculate the discount.</param>
        /// <returns>The absolute amount to deduce from the order.</returns>
        decimal GetDiscount(TType obj);
    }
}