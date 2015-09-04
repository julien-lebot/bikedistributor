using System.Collections.Generic;
using BikeDistributor.Models;

namespace BikeDistributor
{
    public interface IDiscountPolicy
    {
        IEnumerable<decimal> GetDiscounts(Order order);
    }
}