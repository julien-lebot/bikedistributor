using System.Collections.Generic;

namespace BikeDistributor
{
    public class Order
    {
        private readonly IReceiptBuilder _receiptBuilder;
        private readonly decimal _taxRate;
        private readonly IList<Line> _lines = new List<Line>();
        private readonly IList<string> _discountCodes = new List<string>();

        public Order(string company, IReceiptBuilder receiptBuilder, decimal taxRate = .0725m)
        {
            _receiptBuilder = receiptBuilder;
            _taxRate = taxRate;
            Company = company;
        }

        public string Company { get; private set; }

        public IList<Line> Lines
        {
            get
            {
                return _lines;
            }
        }

        public IList<string> DiscountCodes
        {
            get
            {
                return _discountCodes;
            }
        }

        public decimal TaxRate
        {
            get
            {
                return _taxRate;
            }
        }

        public void AddDiscountCode(string code)
        {
            _discountCodes.Add(code);
        }

        public void AddLine(Line line)
        {
            _lines.Add(line);
        }

        public string Receipt()
        {
            return _receiptBuilder.GenerateReceipt(this);
        }
    }
}
