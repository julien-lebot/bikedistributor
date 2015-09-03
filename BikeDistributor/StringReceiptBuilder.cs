using System;
using System.Linq;
using System.Text;

namespace BikeDistributor
{
    public class StringReceiptBuilder : IReceiptBuilder
    {
        private readonly IDiscountPolicy _discountPolicy;

        public StringReceiptBuilder(IDiscountPolicy discountPolicy)
        {
            _discountPolicy = discountPolicy;
        }

        public string GenerateReceipt(Order order)
        {
            var totalAmount = 0m;
            var result = new StringBuilder(string.Format("Order Receipt for {0}{1}", order.Company, Environment.NewLine));
            var discounts = _discountPolicy.GetDiscounts(order).ToList();
            foreach (var line in order.Lines)
            {
                var thisAmount = line.Quantity * line.Bike.Price * discounts[order.Lines.IndexOf(line)];
                result.AppendLine(string.Format("\t{0} x {1} {2} = {3}", line.Quantity, line.Bike.Brand, line.Bike.Model, thisAmount.ToString("C")));
                totalAmount += thisAmount;
            }
            result.AppendLine(string.Format("Sub-Total: {0}", totalAmount.ToString("C")));
            var tax = totalAmount * order.TaxRate;
            result.AppendLine(string.Format("Tax: {0}", tax.ToString("C")));
            result.Append(string.Format("Total: {0}", (totalAmount + tax).ToString("C")));
            return result.ToString();           
        }
    }
}