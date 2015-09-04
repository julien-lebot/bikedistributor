using System.Collections.Generic;

namespace BikeDistributor.Models
{
    public class Order
    {
        public Order(string company, string currency,  decimal taxRate)
        {
            Currency = currency;
            TaxRate = taxRate;
            Company = company;
            Lines = new List<Line>();
            DiscountCodes = new List<string>();
        }

        public string Currency
        {
            get;
            set;
        }

        public string Company { get; private set; }

        public List<Line> Lines
        {
            get;
            set;
        }

        public List<string> DiscountCodes
        {
            get;
            set;
        }

        public decimal TaxRate
        {
            get;
            set;
        }
    }

}
