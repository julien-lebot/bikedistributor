using System;
using BikeDistributor.DiscountCalculators;
using BikeDistributor.Models;
using NSubstitute;
using NUnit.Framework;

namespace BikeDistributor.Test
{
    [TestFixture]
    class PredicateDiscountCalculatorTests
    {
        [Test]
        public void Should_Throw_ArgumentNullException_When_Setting_Null_Calculator()
        {
            var sut = new PredicateDiscountCalculator<Line>();

            Assert.Throws<ArgumentNullException>(() => sut.DiscountCalculator = null);
        }

        [Test]
        public void Should_GetDiscount_Throw_InvalidOperationException_When_Calculator_Is_Null()
        {
            var sut = new PredicateDiscountCalculator<Line>();

            Assert.Throws<InvalidOperationException>(() => sut.GetDiscount(null));
        }

        [Test]
        public void Should_GetDiscount_Always_Execute_Calculator_When_Predicate_Null()
        {
            var calculator = Substitute.For<Func<Line, decimal>>();

            var sut = new PredicateDiscountCalculator<Line> {DiscountCalculator = calculator};
            sut.GetDiscount(null);

            calculator.Received(1).Invoke(Arg.Any<Line>());
        }

        [Test]
        public void GetDiscount_Should_Not_Execute_Calculator_When_Predicate_False()
        {
            var calculator = Substitute.For<Func<Line, decimal>>();
            
            var sut = new PredicateDiscountCalculator<Line> { DiscountCalculator = calculator, Predicate = _ => false};
            sut.GetDiscount(null);

            calculator.DidNotReceive().Invoke(Arg.Any<Line>());
        }
    }
}
