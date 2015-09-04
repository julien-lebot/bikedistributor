using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeDistributor.Helpers;
using NUnit.Framework;

namespace BikeDistributor.Test
{
    [TestFixture]
    class CurrencyToolsTests
    {
        [TestCase("unicorn")]
        public void Should_FormatCurrency_Throw_ArgumentException_With_Invalid_CurrencyCode(string code)
        {
            Assert.Throws<ArgumentException>(() => 0.0m.FormatCurrency(code));
        }

        [TestCase(null)]
        [TestCase("")]
        public void Should_FormatCurrency_Throw_ArgumentNullException_With_Null_Or_Empty_CurrencyCode(string code)
        {
            Assert.Throws<ArgumentNullException>(() => 0.0m.FormatCurrency(code));
        }
    }
}
