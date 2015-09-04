using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BikeDistributor.Models;
using BikeDistributor.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace BikeDistributor.Test
{
    [TestFixture]
    class OrderViewModelTests
    {
        #region Sanity tests
        [Test]
        public void Should_Reflect_Underlying_Model()
        {
            var sut = new OrderViewModel(
                Substitute.For<IDiscountCalculator<OrderViewModel>>(),
                Substitute.For<IDiscountCalculator<LineViewModel>>(),
                new Order("company", "USD", 0.0725m));

            Assert.That(sut.Company, Is.EqualTo("company"));
            Assert.That(sut.Currency, Is.EqualTo("USD"));
            Assert.That(sut.TaxRate, Is.EqualTo(0.0725m));
        }

        [Test]
        public void Should_With_No_Lines_Cost_Nothing()
        {
            var sut = new OrderViewModel(
                Substitute.For<IDiscountCalculator<OrderViewModel>>(),
                Substitute.For<IDiscountCalculator<LineViewModel>>(),
                new Order(Arg.Any<string>(), Arg.Any<string>(), 0.0725m));

            Assert.That(sut.Total, Is.EqualTo(0m));
            Assert.That(sut.SubTotal, Is.EqualTo(0m));
            Assert.That(sut.Tax, Is.EqualTo(0m));
        }
        #endregion

        #region Error handling
        [Test]
        public void Should_Throw_ArgumentNullException_When_Order_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new OrderViewModel(
                Substitute.For<IDiscountCalculator<OrderViewModel>>(),
                Substitute.For<IDiscountCalculator<LineViewModel>>(),
                null));
        }

        [Test]
        public void Should_Not_Throw_ArgumentNullException_When_Line_DiscountCalculator_Is_Null()
        {
            var sut = new OrderViewModel(
                Substitute.For<IDiscountCalculator<OrderViewModel>>(),
                null,
                new Order(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>()));

            sut.AddLine(new Line(new Bike(Arg.Any<string>(), Arg.Any<string>(), 1000m), 1));

            Assert.That(sut.Lines.First().Discount, Is.EqualTo(0));
        }

        [Test]
        public void Should_Not_Throw_ArgumentNullException_When_Order_DiscountCalculator_Is_Null()
        {
            var sut = new OrderViewModel(null, Substitute.For<IDiscountCalculator<LineViewModel>>(),
                new Order(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>()));

            Assert.That(sut.Discount, Is.EqualTo(0));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Adding_Null_line()
        {
            var sut = new OrderViewModel(
                Substitute.For<IDiscountCalculator<OrderViewModel>>(),
                Substitute.For<IDiscountCalculator<LineViewModel>>(),
                new Order(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>()));

            Assert.Throws<ArgumentNullException>(() => sut.AddLine(null));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Adding_Null_DiscountCode()
        {
            var sut = new OrderViewModel(
                Substitute.For<IDiscountCalculator<OrderViewModel>>(),
                Substitute.For<IDiscountCalculator<LineViewModel>>(),
                new Order(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>()));

            Assert.Throws<ArgumentNullException>(() => sut.AddDiscountCode(null));
            Assert.Throws<ArgumentNullException>(() => sut.AddDiscountCode(string.Empty));
        }
        #endregion

        #region Updating order
        [Test]
        public void Should_Update_Prices_When_Adding_Line()
        {
            var sut = new OrderViewModel(
                Substitute.For<IDiscountCalculator<OrderViewModel>>(),
                Substitute.For<IDiscountCalculator<LineViewModel>>(),
                new Order(Arg.Any<string>(), Arg.Any<string>(), 0.0725m));

            Assert.That(sut.Total, Is.EqualTo(0m));
            Assert.That(sut.SubTotal, Is.EqualTo(0m));
            Assert.That(sut.Tax, Is.EqualTo(0m));

            sut.AddLine(new Line(new Bike("brand", "model", 1000m), 1));

            Assert.That(sut.Lines.Count, Is.EqualTo(1));
            Assert.That(sut.SubTotal, Is.EqualTo(1000m));
            Assert.That(sut.Tax, Is.EqualTo(1000m * 0.0725m));
            Assert.That(sut.Total, Is.EqualTo(1000m * 0.0725m + 1000m));
        }

        [Test]
        public void Should_Update_Prices_When_Adding_DiscountCode()
        {
            var orderDiscount = Substitute.For<IDiscountCalculator<OrderViewModel>>();
            orderDiscount.GetDiscount(Arg.Any<OrderViewModel>()).Returns(info =>
            {
                var orderVm = info.ArgAt<OrderViewModel>(0);
                if (orderVm.DiscountCodes != null && orderVm.DiscountCodes.Contains("test"))
                {
                    return orderVm.SubTotal * 0.1m;
                }
                return 0;
            });

            var sut = new OrderViewModel(orderDiscount, Substitute.For<IDiscountCalculator<LineViewModel>>(),
                new Order(Arg.Any<string>(), Arg.Any<string>(), 0.0725m));

            sut.AddLine(new Line(new Bike("brand", "model", 1000m), 1));

            Assert.That(sut.Lines.Count, Is.EqualTo(1));
            Assert.That(sut.SubTotal, Is.EqualTo(1000m));
            Assert.That(sut.Tax, Is.EqualTo(1000m * 0.0725m));
            Assert.That(sut.Total, Is.EqualTo(1000m * 0.0725m + 1000m));

            sut.AddDiscountCode("test");

            Assert.That(sut.SubTotal, Is.EqualTo(1000m));
            Assert.That(sut.Tax, Is.EqualTo(1000m * 0.0725m));
            Assert.That(sut.Total, Is.EqualTo(1000m * 0.0725m + 900m));
        }

        [Test]
        public void Should_Constructor_Update_Prices()
        {
            var order = new Order(Arg.Any<string>(), Arg.Any<string>(), 0.0725m);
            order.Lines.Add(new Line(new Bike(Arg.Any<string>(), Arg.Any<string>(), 1000m), 1));

            var sut = new OrderViewModel(
                Substitute.For<IDiscountCalculator<OrderViewModel>>(),
                Substitute.For<IDiscountCalculator<LineViewModel>>(),
                order);

            Assert.That(sut.Lines.Count, Is.EqualTo(1));
            Assert.That(sut.SubTotal, Is.EqualTo(1000m));
            Assert.That(sut.Tax, Is.EqualTo(1000m * 0.0725m));
            Assert.That(sut.Total, Is.EqualTo(1000m * 0.0725m + 1000m));
        }
        #endregion

        #region Discounts
        [TestCaseSource("LineTestCases")]
        public void Should_Apply_Order_Fixed_Discount(
            IList<Line> lines,
            decimal fixedDiscount, 
           string currency,
            decimal taxRate)
        {
            var discountCalculator = Substitute.For<IDiscountCalculator<OrderViewModel>>();
            discountCalculator.GetDiscount(Arg.Any<OrderViewModel>()).Returns(fixedDiscount);
            var sut = new OrderViewModel(
                discountCalculator,
                Substitute.For<IDiscountCalculator<LineViewModel>>(),
                new Order(Arg.Any<string>(), currency, taxRate));

            foreach (var line in lines)
            {
                sut.AddLine(line);
            }

            var subtotal = lines.Sum(line => line.Quantity * line.Bike.Price);
            var tax = subtotal * taxRate;
            var total = (subtotal - fixedDiscount) + tax;
            Assert.That(sut.Lines.Count, Is.EqualTo(lines.Count));
            Assert.That(sut.SubTotal, Is.EqualTo(subtotal));
            Assert.That(sut.Tax, Is.EqualTo(tax));
            Assert.That(sut.Total, Is.EqualTo(total));
        }

        [TestCaseSource("LineTestCases")]
        public void Should_Apply_Line_Fixed_Discount(
            IList<Line> lines,
            decimal fixedDiscount,
            string currency,
            decimal taxRate)
        {
            var discountCalculator = Substitute.For<IDiscountCalculator<LineViewModel>>();
            discountCalculator.GetDiscount(Arg.Any<LineViewModel>()).Returns(fixedDiscount);
            var sut = new OrderViewModel(
                Substitute.For<IDiscountCalculator<OrderViewModel>>(),
                discountCalculator,
                new Order(Arg.Any<string>(), currency, taxRate));

            foreach (var line in lines)
            {
                sut.AddLine(line);
            }

            var subtotal = lines.Sum(line => line.Quantity * line.Bike.Price - fixedDiscount);
            var tax = subtotal * taxRate;
            var total = subtotal + tax;
            Assert.That(sut.Lines.Count, Is.EqualTo(lines.Count));
            Assert.That(sut.SubTotal, Is.EqualTo(subtotal));
            Assert.That(sut.Tax, Is.EqualTo(tax));
            Assert.That(sut.Total, Is.EqualTo(total));
        }

        public static IEnumerable LineTestCases
        {
            get
            {
                var rand = new Random(42);

                var currencies = new string[] {"USD", "EUR", "JPY"};

                // One line
                yield return new TestCaseData(new List<Line>
                    {
                        new Line(
                            new Bike(
                                Arg.Any<string>(),
                                Arg.Any<string>(),
                                rand.Next(0, 10000)
                             ),
                             rand.Next(0, 500)
                        )
                    },
                    (decimal)rand.NextDouble(),
                    currencies[rand.Next(0, currencies.Count())],
                    (decimal)rand.NextDouble());

                // Two lines
                yield return new TestCaseData(new List<Line>
                    {
                        new Line(
                            new Bike(
                                Arg.Any<string>(),
                                Arg.Any<string>(),
                                rand.Next(0, 10000)
                            ),
                            rand.Next(0, 500)
                        ),
                        new Line(
                            new Bike(
                                Arg.Any<string>(),
                                Arg.Any<string>(),
                                rand.Next(0, 10000)
                            ),
                            rand.Next(0, 500)
                        )
                    },
                    (decimal)rand.NextDouble(),
                    currencies[rand.Next(0, currencies.Count())],
                    (decimal)rand.NextDouble());
            }
        }
        #endregion
    }
}
