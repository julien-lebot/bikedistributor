using System;
using BikeDistributor.DiscountCalculators;
using BikeDistributor.Models;
using NSubstitute;
using NUnit.Framework;

namespace BikeDistributor.Test
{
    [TestFixture]
    class DiscountStrategyCalculator
    {
        #region Error Handling
        [Test]
        public void Should_AddCalculator_Throw_ArgumentException_When_Cyclic_References()
        {
            var sut = new DiscountStrategyCalculator<Order>();

            Assert.Throws<ArgumentException>(() => sut.AddCalculator(sut));
        }
        #endregion

        [Test]
        public void Should_AddCalculator_Compose_With_Itself()
        {
            var calculator = new DiscountStrategyCalculator<Order>(DiscountStrategyOperations.Sum);
            var otherCalculator = new DiscountStrategyCalculator<Order>();

            calculator.Configure()
                .When(_ => true)
                .ApplyDiscount(_ => 42m);

            otherCalculator.Configure()
                .When(_ => true)
                .ApplyDiscount(_ => 42m);

            calculator.AddCalculator(otherCalculator);

            Assert.That(
                calculator.GetDiscount(new Order(Arg.Any<string>(), Arg.Any<string>(), 0.0725m)),
                Is.EqualTo(84m));
        }

        #region Discount operations
        [TestCase]
        public void Should_GetDiscount_With_SumOperation_Sums_Discounts()
        {
            var sut = new DiscountStrategyCalculator<Order>(DiscountStrategyOperations.Sum);

            sut.Configure()
                .When(_ => true)
                .ApplyDiscount(_ => 20m)
                .When(_ => true)
                .ApplyDiscount(_ => 60m);

            var discount = sut.GetDiscount(new Order(Arg.Any<string>(), Arg.Any<string>(), 0.0725m));

            Assert.That(discount, Is.EqualTo(80m));
        }

        [Test]
        public void Should_GetDiscount_With_MaxOperation_Maxs_Discounts()
        {
            var sut = new DiscountStrategyCalculator<Order>(DiscountStrategyOperations.Max);

            sut.Configure()
                .When(_ => true)
                .ApplyDiscount(_ => 20m)
                .When(_ => true)
                .ApplyDiscount(_ => 60m);

            var discount = sut.GetDiscount(new Order(Arg.Any<string>(), Arg.Any<string>(), 0.0725m));

            Assert.That(discount, Is.EqualTo(60m));
        }

        [Test]
        public void Should_GetDiscount_With_MinOperation_Mins_Discounts()
        {
            var sut = new DiscountStrategyCalculator<Order>(DiscountStrategyOperations.Min);

            sut.Configure()
                .When(_ => true)
                .ApplyDiscount(_ => 20m)
                .When(_ => true)
                .ApplyDiscount(_ => 60m);

            var discount = sut.GetDiscount(new Order(Arg.Any<string>(), Arg.Any<string>(), 0.0725m));

            Assert.That(discount, Is.EqualTo(20m));
        }
        #endregion
    }
}
