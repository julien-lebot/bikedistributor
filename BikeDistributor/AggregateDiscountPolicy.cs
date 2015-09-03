using System.Collections.Generic;
using System.Linq;

namespace BikeDistributor
{
    /// <summary>
    /// A discount policy that aggregates multiple discount strategies
    /// </summary>
    public class AggregateDiscountPolicy : IDiscountPolicy
    {
        private readonly IList<IDiscountStrategy> _discountStrategies = new List<IDiscountStrategy>();

        public void AddStrategy(IDiscountStrategy strategy)
        {
            _discountStrategies.Add(strategy);
        }

        public IEnumerable<decimal> GetDiscounts(Order order)
        {
            return order.Lines.Select(line => _discountStrategies.Min(strategy => strategy.GetDiscount(line)));
        }
    }
}