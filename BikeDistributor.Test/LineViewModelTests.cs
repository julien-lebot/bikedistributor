using System;
using BikeDistributor.Models;
using BikeDistributor.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace BikeDistributor.Test
{
    [TestFixture]
    class LineViewModelTests
    {
        #region Error handling
        [Test]
        public void Should_Throw_ArgumentNullException_When_Line_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new LineViewModel(
                null,
                Substitute.For<IDiscountCalculator<LineViewModel>>()));
        }

        [Test]
        public void Should_Not_Throw_ArgumentNullException_When_Discount_Calculator_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
             new LineViewModel(new Line(
                new Bike(Arg.Any<string>(), Arg.Any<string>(), 1000m), 1), null);
        }
        
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(0)]
        public void Should_Throw_ArgumentException_When_Line_Quantity_Is_Lower_Or_Equal_To_Zero(int quantity)
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => new LineViewModel(new Line(
                new Bike(Arg.Any<string>(), Arg.Any<string>(), 1000m), quantity),
                Substitute.For<IDiscountCalculator<LineViewModel>>()));
        }

        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(0)]
        public void Should_Throw_ArgumentException_When_Line_Price_Is_Lower_Or_Equal_To_Zero(decimal price)
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => new LineViewModel(new Line(
                new Bike(Arg.Any<string>(), Arg.Any<string>(), price), 1),
                Substitute.For<IDiscountCalculator<LineViewModel>>()));
        }
        #endregion

        [Test]
        public void Should_Amount_Include_Discount()
        {
            var discount = Substitute.For<IDiscountCalculator<LineViewModel>>();
            discount.GetDiscount(Arg.Any<LineViewModel>()).Returns(100m);

            var sut = new LineViewModel(
                new Line(
                    new Bike(
                        Arg.Any<string>(),
                        Arg.Any<string>(),
                        1000m
                    ),
                    1
                ),
                discount);

            Assert.That(sut.Discount, Is.EqualTo(100m));
            Assert.That(sut.Total, Is.EqualTo(900m));
        }
    }
}
