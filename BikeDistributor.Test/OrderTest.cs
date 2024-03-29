﻿using System.Globalization;
using System.Threading;
using BikeDistributor.DiscountCalculators;
using BikeDistributor.Models;
using BikeDistributor.ReceiptBuilders;
using BikeDistributor.ViewModels;
using NSubstitute;
using NUnit.Framework;

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

        [TestCase(1, ExpectedResult = "Order Receipt for Anywhere Bike Shop\r\n\t1 x Giant Defy 1 = $1,000.00\r\nSub-Total: $1,000.00\r\nTax: $72.50\r\nTotal: $1,072.50")]
        [TestCase(19, ExpectedResult = "Order Receipt for Anywhere Bike Shop\r\n\t19 x Giant Defy 1 = $19,000.00\r\nSub-Total: $19,000.00\r\nTax: $1,377.50\r\nTotal: $20,377.50")]
        [TestCase(20, ExpectedResult = "Order Receipt for Anywhere Bike Shop\r\n\t20 x Giant Defy 1 = $18,000.00\r\nSub-Total: $18,000.00\r\nTax: $1,305.00\r\nTotal: $19,305.00")]
        [TestCase(21, ExpectedResult = "Order Receipt for Anywhere Bike Shop\r\n\t21 x Giant Defy 1 = $18,900.00\r\nSub-Total: $18,900.00\r\nTax: $1,370.25\r\nTotal: $20,270.25")]
        public string ReceiptOneDefy(int amount)
        {
            var lineDiscount = new DiscountStrategyCalculator<LineViewModel>();
            lineDiscount.Configure()
                  .When(line => line.Quantity >= 20 && line.Price >= 1000)
                  .ApplyDiscount(line => line.Quantity * line.Price * 0.1m);
            var orderVm = new OrderViewModel(Substitute.For<IDiscountCalculator<OrderViewModel>>(), lineDiscount, new Order("Anywhere Bike Shop", "USD", 0.0725m));
            orderVm.AddLine(new Line(Defy, amount));
            var generator = new StringReceiptBuilder();
            return generator.GenerateReceipt(orderVm);
        }

        [Test]
        public void ReceiptOneElite()
        {
            var order = new OrderViewModel(Substitute.For<IDiscountCalculator<OrderViewModel>>(), Substitute.For<IDiscountCalculator<LineViewModel>>(), new Order("Anywhere Bike Shop", "USD", 0.0725m));
            order.AddLine(new Line(Elite, 1));
            var generator = new StringReceiptBuilder();
            Assert.AreEqual(ResultStatementOneElite, generator.GenerateReceipt(order));
        }

        private const string ResultStatementOneElite = @"Order Receipt for Anywhere Bike Shop
	1 x Specialized Venge Elite = $2,000.00
Sub-Total: $2,000.00
Tax: $145.00
Total: $2,145.00";

        [Test]
        public void ReceiptOneDuraAce()
        {
            var order = new OrderViewModel(Substitute.For<IDiscountCalculator<OrderViewModel>>(), Substitute.For<IDiscountCalculator<LineViewModel>>(), new Order("Anywhere Bike Shop", "USD", 0.0725m));
            order.AddLine(new Line(DuraAce, 1));
            var generator = new StringReceiptBuilder();
            Assert.AreEqual(ResultStatementOneDuraAce, generator.GenerateReceipt(order));
        }

        private const string ResultStatementOneDuraAce = @"Order Receipt for Anywhere Bike Shop
	1 x Specialized S-Works Venge Dura-Ace = $5,000.00
Sub-Total: $5,000.00
Tax: $362.50
Total: $5,362.50";

        [Test]
        public void HtmlReceiptOneDefy()
        {
            var order = new OrderViewModel(Substitute.For<IDiscountCalculator<OrderViewModel>>(), Substitute.For<IDiscountCalculator<LineViewModel>>(), new Order("Anywhere Bike Shop", "USD", 0.0725m));
            order.AddLine(new Line(Defy, 1));
            var generator = HtmlReceiptBuilder.TestBuilder();
            Assert.AreEqual(HtmlResultStatementOneDefy, generator.GenerateReceipt(order));
        }

        private const string HtmlResultStatementOneDefy = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Giant Defy 1 = $1,000.00</li></ul><h3>Sub-Total: $1,000.00</h3><h3>Tax: $72.50</h3><h2>Total: $1,072.50</h2></body></html>";

        [Test]
        public void HtmlReceiptOneElite()
        {
            var order = new OrderViewModel(Substitute.For<IDiscountCalculator<OrderViewModel>>(), Substitute.For<IDiscountCalculator<LineViewModel>>(), new Order("Anywhere Bike Shop", "USD", 0.0725m));
            order.AddLine(new Line(Elite, 1));
            var generator = HtmlReceiptBuilder.TestBuilder();
            Assert.AreEqual(HtmlResultStatementOneElite, generator.GenerateReceipt(order));
        }

        private const string HtmlResultStatementOneElite = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Specialized Venge Elite = $2,000.00</li></ul><h3>Sub-Total: $2,000.00</h3><h3>Tax: $145.00</h3><h2>Total: $2,145.00</h2></body></html>";

        [Test]
        public void HtmlReceiptOneDuraAce()
        {
            var order = new OrderViewModel(Substitute.For<IDiscountCalculator<OrderViewModel>>(), Substitute.For<IDiscountCalculator<LineViewModel>>(), new Order("Anywhere Bike Shop", "USD", 0.0725m));
            order.AddLine(new Line(DuraAce, 1));
            var generator = HtmlReceiptBuilder.TestBuilder();
            Assert.AreEqual(HtmlResultStatementOneDuraAce, generator.GenerateReceipt(order));
        }

        private const string HtmlResultStatementOneDuraAce = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Specialized S-Works Venge Dura-Ace = $5,000.00</li></ul><h3>Sub-Total: $5,000.00</h3><h3>Tax: $362.50</h3><h2>Total: $5,362.50</h2></body></html>";    
    }
}


