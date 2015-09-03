using System.Linq;
using System.Text;

namespace BikeDistributor
{
    public class HtmlReceiptBuilder : IReceiptBuilder
    {
        private readonly IDiscountPolicy _discountPolicy;

        public HtmlReceiptBuilder(IDiscountPolicy discountPolicy)
        {
            _discountPolicy = discountPolicy;
        }

        public string GenerateReceipt(Order order)
        {
            var totalAmount = 0m;
            var result = new StringBuilder(string.Format("<html><body><h1>Order Receipt for {0}</h1>", order.Company));
            if (order.Lines.Any())
            {
                result.Append("<ul>");
                var discounts = _discountPolicy.GetDiscounts(order).ToList();
                foreach (var line in order.Lines)
                {
                    var thisAmount = line.Quantity * line.Bike.Price * discounts[order.Lines.IndexOf(line)];
                    result.Append(string.Format("<li>{0} x {1} {2} = {3}</li>", line.Quantity, line.Bike.Brand, line.Bike.Model, thisAmount.ToString("C")));
                    totalAmount += thisAmount;
                }
                result.Append("</ul>");
            }
            result.Append(string.Format("<h3>Sub-Total: {0}</h3>", totalAmount.ToString("C")));
            var tax = totalAmount * order.TaxRate;
            result.Append(string.Format("<h3>Tax: {0}</h3>", tax.ToString("C")));
            result.Append(string.Format("<h2>Total: {0}</h2>", (totalAmount + tax).ToString("C")));
            result.Append("</body></html>");
            return result.ToString();          
        }
    }
}