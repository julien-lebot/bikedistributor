using System.Collections.Generic;

namespace BikeDistributor
{
    public interface IDiscountPolicy
    {
        IEnumerable<decimal> GetDiscounts(Order order);
    }
}