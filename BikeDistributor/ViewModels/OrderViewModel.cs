using System;
using System.Collections.Generic;
using System.Linq;
using BikeDistributor.Models;

namespace BikeDistributor.ViewModels
{
    public class OrderViewModel
    {
        private readonly IDiscountCalculator<OrderViewModel> _orderDiscountCalculator;
        private readonly IDiscountCalculator<LineViewModel> _lineDiscountCalculator;
        private readonly Order _order;
        private readonly List<LineViewModel> _lines;

        public OrderViewModel(IDiscountCalculator<OrderViewModel> orderDiscountCalculator, IDiscountCalculator<LineViewModel> lineDiscountCalculator, Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            _orderDiscountCalculator = orderDiscountCalculator;
            _lineDiscountCalculator = lineDiscountCalculator;
            _order = order;
            _lines = new List<LineViewModel>(_order.Lines.Select(line => new LineViewModel(line, _lineDiscountCalculator)));
            UpdatePrices();
        }

        public IReadOnlyCollection<LineViewModel> Lines
        {
            get
            {
                return _lines;
            }
        }

        private void UpdatePrices()
        {
            SubTotal = Lines.Sum(item => item.Total);
            Tax = _order.TaxRate * SubTotal;
            _discount = _orderDiscountCalculator != null ? _orderDiscountCalculator.GetDiscount(this) : 0;
        }

        private decimal _discount;
        public decimal Discount
        {
            get
            {
                return _discount;
            }
        }

        public IReadOnlyCollection<string> DiscountCodes
        {
            get
            {
                return _order.DiscountCodes;
            }
        }

        public string Company
        {
            get
            {
                return _order.Company;
            }
        }

        public decimal SubTotal
        {
            get;
            private set;
        }

        public decimal Total
        {
            get
            {
                return (SubTotal - Discount) + Tax;
            }
        }

        public decimal Tax
        {
            get;
            private set;
        }

        public string Currency
        {
            get
            {
                return _order.Currency;
            }
        }

        public decimal TaxRate
        {
            get
            {
                return _order.TaxRate;
            }
        }

        public void AddDiscountCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code");
            }
            _order.DiscountCodes.Add(code);
            UpdatePrices();
        }

        public void AddLine(Line line)
        {
            _lines.Add(new LineViewModel(line, _lineDiscountCalculator));
            _order.Lines.Add(line);
            UpdatePrices();
        }
    }
}