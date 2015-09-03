using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BikeDistributor.Test
{
    [TestFixture]
    public class OrderTest
    {
        private readonly static Bike Defy = new Bike("Giant", "Defy 1", 1000);
        private readonly static Bike Elite = new Bike("Specialized", "Venge Elite", 2000);
        private readonly static Bike DuraAce = new Bike("Specialized", "S-Works Venge Dura-Ace", 5000);

        [SetUp]
        public void Init()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        public IDiscountPolicy CreateStandardDiscountPolicy()
        {
            var policy = new DiscountPolicy();

            var strategies = new IDiscountStrategy[3]
            {
                Substitute.For<IDiscountStrategy>(),
                Substitute.For<IDiscountStrategy>(),
                Substitute.For<IDiscountStrategy>()
            };

            // 10% discount on bikes over 1000 for quantity >= 20
            strategies[0].GetDiscount(Arg.Any<Line>()).Returns(
                info =>
                {
                    var line = info.ArgAt<Line>(0);
                    if (line.Quantity >= 20 && line.Bike.Price >= 1000)
                    {
                        return 0.9m;
                    }
                    return 1m;
                });

            // 20% discount on bikes over 2000 for quantity >= 10
            strategies[1].GetDiscount(Arg.Any<Line>()).Returns(
                info =>
                {
                    var line = info.ArgAt<Line>(0);
                    if (line.Quantity >= 10 && line.Bike.Price >= 2000)
                    {
                        return 0.8m;
                    }
                    return 1m;
                });

            // 20% discount on bikes over 5000 for quantity >= 5
            strategies[2].GetDiscount(Arg.Any<Line>()).Returns(
                info =>
                {
                    var line = info.ArgAt<Line>(0);
                    if (line.Quantity >= 5 && line.Bike.Price >= 5000)
                    {
                        return 0.8m;
                    }
                    return 1m;
                });

            foreach (var strategy in strategies)
            {
                policy.AddStrategy(strategy);
            }

            return policy;
        }

        [TestCase(1, ExpectedResult = "Order Receipt for Anywhere Bike Shop\r\n\t1 x Giant Defy 1 = $1,000.00\r\nSub-Total: $1,000.00\r\nTax: $72.50\r\nTotal: $1,072.50")]
        [TestCase(19, ExpectedResult = "Order Receipt for Anywhere Bike Shop\r\n\t19 x Giant Defy 1 = $19,000.00\r\nSub-Total: $19,000.00\r\nTax: $1,377.50\r\nTotal: $20,377.50")]
        [TestCase(20, ExpectedResult = "Order Receipt for Anywhere Bike Shop\r\n\t20 x Giant Defy 1 = $18,000.00\r\nSub-Total: $18,000.00\r\nTax: $1,305.00\r\nTotal: $19,305.00")]
        [TestCase(21, ExpectedResult = "Order Receipt for Anywhere Bike Shop\r\n\t21 x Giant Defy 1 = $18,900.00\r\nSub-Total: $18,900.00\r\nTax: $1,370.25\r\nTotal: $20,270.25")]
        public string ReceiptOneDefy(int amount)
        {
            var policy = new DiscountPolicy();
            var discount = Substitute.For<IDiscountStrategy>();
            discount.GetDiscount(Arg.Any<Line>()).Returns(
                info =>
                {
                    var line = info.ArgAt<Line>(0);
                    if (line.Quantity >= 20 && line.Bike.Price >= 1000)
                    {
                        return 0.9m;
                    }
                    return 1m;
                });
            policy.AddStrategy(discount);

            var order = new Order("Anywhere Bike Shop", new StringReceiptBuilder(policy));
            order.AddLine(new Line(Defy, amount));
            return order.Receipt();
        }

        private const string ResultStatementOneDefy = @"Order Receipt for Anywhere Bike Shop
	1 x Giant Defy 1 = $1,000.00
Sub-Total: $1,000.00
Tax: $72.50
Total: $1,072.50";

        [TestMethod]
        public void ReceiptOneElite()
        {
            var order = new Order("Anywhere Bike Shop", new StringReceiptBuilder(CreateStandardDiscountPolicy()));
            order.AddLine(new Line(Elite, 1));
            Assert.AreEqual(ResultStatementOneElite, order.Receipt());
        }

        private const string ResultStatementOneElite = @"Order Receipt for Anywhere Bike Shop
	1 x Specialized Venge Elite = $2,000.00
Sub-Total: $2,000.00
Tax: $145.00
Total: $2,145.00";

        [TestMethod]
        public void ReceiptOneDuraAce()
        {
            var order = new Order("Anywhere Bike Shop", new StringReceiptBuilder(CreateStandardDiscountPolicy()));
            order.AddLine(new Line(DuraAce, 1));
            Assert.AreEqual(ResultStatementOneDuraAce, order.Receipt());
        }

        private const string ResultStatementOneDuraAce = @"Order Receipt for Anywhere Bike Shop
	1 x Specialized S-Works Venge Dura-Ace = $5,000.00
Sub-Total: $5,000.00
Tax: $362.50
Total: $5,362.50";

        [TestMethod]
        public void HtmlReceiptOneDefy()
        {
            var order = new Order("Anywhere Bike Shop", new HtmlReceiptBuilder(CreateStandardDiscountPolicy()));
            order.AddLine(new Line(Defy, 1));
            Assert.AreEqual(HtmlResultStatementOneDefy, order.Receipt());
        }

        private const string HtmlResultStatementOneDefy = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Giant Defy 1 = $1,000.00</li></ul><h3>Sub-Total: $1,000.00</h3><h3>Tax: $72.50</h3><h2>Total: $1,072.50</h2></body></html>";

        [TestMethod]
        public void HtmlReceiptOneElite()
        {
            var order = new Order("Anywhere Bike Shop", new HtmlReceiptBuilder(CreateStandardDiscountPolicy()));
            order.AddLine(new Line(Elite, 1));
            Assert.AreEqual(HtmlResultStatementOneElite, order.Receipt());
        }

        private const string HtmlResultStatementOneElite = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Specialized Venge Elite = $2,000.00</li></ul><h3>Sub-Total: $2,000.00</h3><h3>Tax: $145.00</h3><h2>Total: $2,145.00</h2></body></html>";

        [TestMethod]
        public void HtmlReceiptOneDuraAce()
        {
            var order = new Order("Anywhere Bike Shop", new HtmlReceiptBuilder(CreateStandardDiscountPolicy()));
            order.AddLine(new Line(DuraAce, 1));
            Assert.AreEqual(HtmlResultStatementOneDuraAce, order.Receipt());
        }

        private const string HtmlResultStatementOneDuraAce = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Specialized S-Works Venge Dura-Ace = $5,000.00</li></ul><h3>Sub-Total: $5,000.00</h3><h3>Tax: $362.50</h3><h2>Total: $5,362.50</h2></body></html>";    
    }
}


