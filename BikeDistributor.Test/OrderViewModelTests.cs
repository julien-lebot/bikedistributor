using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void Should_OrderVm_Reflect_Underlying_Model()
        {
            var discountCalculator = Substitute.For<IDiscountCalculator>();
            var sut = new OrderViewModel(discountCalculator, new Order("company", "USD", 0.0725m));

            Assert.That(sut.Company, Is.EqualTo("company"));
            Assert.That(sut.Currency, Is.EqualTo("USD"));
            Assert.That(sut.TaxRate, Is.EqualTo(0.0725m));
        }

        [Test]
        public void Should_OrderVm_With_No_Lines_Cost_Nothing()
        {
            var discountCalculator = Substitute.For<IDiscountCalculator>();
            var sut = new OrderViewModel(discountCalculator, new Order("company", "USD", 0.0725m));

            Assert.That(sut.Total, Is.EqualTo(0m));
            Assert.That(sut.SubTotal, Is.EqualTo(0m));
            Assert.That(sut.Tax, Is.EqualTo(0m));
        }
        #endregion

        #region Error handling
        [Test]
        public void Should_OrderVm_Throw_ArgumentNullException_When_Order_Is_Null()
        {
            var discountCalculator = Substitute.For<IDiscountCalculator>();
            Assert.Throws<ArgumentNullException>(() => new OrderViewModel(discountCalculator, null));
        }

        [Test]
        public void Should_OrderVm_Throw_ArgumentNullException_When_DiscountCalculator_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderViewModel(null, new Order("company", "USD", 0.0725m)));
        }

        [Test]
        public void Should_OrderVm_Throw_ArgumentNullException_When_Adding_Null_line()
        {
            var discountCalculator = Substitute.For<IDiscountCalculator>();
            var sut = new OrderViewModel(discountCalculator, new Order("company", "USD", 0.0725m));

            Assert.Throws<ArgumentNullException>(() => sut.AddLine(null));
        }
        #endregion

        #region Adding line to order
        [Test]
        public void Should_OrderVm_Update_Prices_When_Adding_Line()
        {
            var discountCalculator = Substitute.For<IDiscountCalculator>();
            var sut = new OrderViewModel(discountCalculator, new Order("company", "USD", 0.0725m));

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
        public void Should_OrderVm_Constructor_Update_Prices()
        {
            var discountCalculator = Substitute.For<IDiscountCalculator>();
            var order = new Order("company", "USD", 0.0725m);
            order.Lines.Add(new Line(new Bike("brand", "model", 1000m), 1));

            var sut = new OrderViewModel(discountCalculator, order);

            Assert.That(sut.Lines.Count, Is.EqualTo(1));
            Assert.That(sut.SubTotal, Is.EqualTo(1000m));
            Assert.That(sut.Tax, Is.EqualTo(1000m * 0.0725m));
            Assert.That(sut.Total, Is.EqualTo(1000m * 0.0725m + 1000m));
        }
        #endregion
    }
}
