using System;
using System.Text;
using BikeDistributor.Helpers;
using BikeDistributor.ViewModels;

namespace BikeDistributor
{
    public class StringReceiptBuilder : IReceiptBuilder
    {
        public string GenerateReceipt(OrderViewModel order)
        {
            var result = new StringBuilder(string.Format("Order Receipt for {0}{1}", order.Company, Environment.NewLine));
            foreach (var line in order.Lines)
            {
                result.AppendLine(string.Format("\t{0} x {1} {2} = {3}", line.Quantity, line.Brand, line.Model, line.Amount.FormatCurrency(order.Currency)));
            }
            result.AppendLine(string.Format("Sub-Total: {0}", order.SubTotal.FormatCurrency(order.Currency)));
            result.AppendLine(string.Format("Tax: {0}", order.Tax.FormatCurrency(order.Currency)));
            result.Append(string.Format("Total: {0}", order.Total.FormatCurrency(order.Currency)));
            return result.ToString();           
        }
    }
}