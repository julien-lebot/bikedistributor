using System.Collections.Generic;
using System.Linq;
using BikeDistributor.Models;

namespace BikeDistributor
{
    /// <summary>
    /// A discount calculator that delegates discount calculations to multiple strategies.
    /// </summary>
    public class StrategyDiscountCalculator : IDiscountCalculator
    {
        private readonly IList<ILineDiscountStrategy> _lineDiscountStrategies = new List<ILineDiscountStrategy>();
        private readonly IList<IOrderDiscountStrategy> _orderDiscountStrategies = new List<IOrderDiscountStrategy>();

        public void AddStrategy(ILineDiscountStrategy strategy)
        {
            _lineDiscountStrategies.Add(strategy);
        }

        public void AddStrategy(IOrderDiscountStrategy strategy)
        {
            _orderDiscountStrategies.Add(strategy);
        }

        public decimal GetDiscount(Order order)
        {
            return _orderDiscountStrategies.Sum(strategy => strategy.GetDiscount(order));
        }

        public decimal GetDiscount(Line line)
        {
            return _lineDiscountStrategies.Sum(strategy => line.Quantity * line.Bike.Price * strategy.GetDiscount(line));
        }
    }
}