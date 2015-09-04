using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RazorEngine.Text;

namespace BikeDistributor.Helpers
{
    public static class CurrencyTools
    {
        private static readonly Dictionary<string, CultureInfo> CurrencyCodeToCultureInfoCache = new Dictionary<string, CultureInfo>();

        public static string FormatCurrency(this decimal amount, string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentNullException("currencyCode");
            }

            CultureInfo cultureInfo = null;
            if (!CurrencyCodeToCultureInfoCache.TryGetValue(currencyCode, out cultureInfo))
            {
                var info = (from c in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                                  let r = new RegionInfo(c.LCID)
                                                  where r != null && String.Equals(r.ISOCurrencySymbol, currencyCode, StringComparison.CurrentCultureIgnoreCase)
                                                  select c).FirstOrDefault();
                if (info == null)
                {
                    throw new ArgumentException("Invalid currency code");
                }
                CurrencyCodeToCultureInfoCache[currencyCode] = info;
            }

            if (!CurrencyCodeToCultureInfoCache.TryGetValue(currencyCode, out cultureInfo))
            {
                return amount.ToString("0.00");
            }
            return string.Format(cultureInfo, "{0:C}", amount);
        }
    }
}