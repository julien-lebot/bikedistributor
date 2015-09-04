using System.IO;
using System.Linq;
using BikeDistributor.DiscountCalculators;
using BikeDistributor.Models;
using BikeDistributor.ReceiptBuilders;
using BikeDistributor.ViewModels;

namespace BikeDistributor.Integration.Demo.RazorHtml
{
    class Program
    {
        static void Main(string[] args)
        {
            var orderDiscountCalculator = new DiscountStrategyCalculator<OrderViewModel>(DiscountStrategyOperations.Sum);
            orderDiscountCalculator.Configure()

                // Our good friends at contoso get a 20% fixed rebate
                .When(order => order.Company == "Contoso")
                .ApplyDiscount(order => order.SubTotal * 0.2m)

                // 40% discount with FOO-BAR code
                .When(order => order.DiscountCodes != null && order.DiscountCodes.Contains("FOO-BAR"))
                .ApplyDiscount(order => order.SubTotal * 0.4m);

            var lineDiscountCalculator = new DiscountStrategyCalculator<LineViewModel>(DiscountStrategyOperations.Max);
            lineDiscountCalculator.Configure()

                // 5% off Giant bikes
                .When(line => line.Brand == "Giant")
                .ApplyDiscount(line => line.SubTotal * 0.05m)

                // 10% off order of 20 or more bikes with a unit price of at least $1000
                .When(line => line.Quantity >= 20 && line.Price >= 1000m)
                .ApplyDiscount(line => line.SubTotal * 0.1m)

                // 20% off order of 10 or more bikes with a unit price of at least $2000
                .When(line => line.Quantity >= 10 && line.Price >= 2000m)
                .ApplyDiscount(line => line.SubTotal * 0.2m)

                // 20% off order of 5 or more bikes with a unit price of at least $5000
                .When(line => line.Quantity >= 5 && line.Price >= 5000m)
                .ApplyDiscount(line => line.SubTotal * 0.2m);

            var orderVm = new OrderViewModel(orderDiscountCalculator, lineDiscountCalculator, new Order("Contoso", "USD" /* GBP etc..., razor seems to have issues with EUR */, 0.0725m));

            orderVm.AddLine(new Line(new Bike("Giant", "Defy 1", 1000), 10));
            orderVm.AddLine(new Line(new Bike("Specialized", "Venge Elite", 2000), 7));
            orderVm.AddLine(new Line(new Bike("Specialized", "S-Works Venge Dura-Ace", 5000), 5));
            //orderVm.AddDiscountCode("FOO-BAR");

            var receiptBuilder = new HtmlReceiptBuilder(File.ReadAllText("test.cshtml"), "test.cshtml");

            File.WriteAllText("test.html", receiptBuilder.GenerateReceipt(orderVm));
        }
    }
}
