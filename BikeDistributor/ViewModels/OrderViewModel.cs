using System;
using System.Collections.Generic;
using System.Linq;
using BikeDistributor.Models;

namespace BikeDistributor.ViewModels
{
    public class OrderViewModel
    {
        private readonly IDiscountCalculator _discountCalculator;
        private readonly Order _order;
        private readonly List<LineViewModel> _lines;

        public OrderViewModel(IDiscountCalculator discountCalculator, Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }
            
            if (discountCalculator == null)
            {
                throw new ArgumentNullException("discountCalculator");
            }

            _discountCalculator = discountCalculator;
            _order = order;
            _lines = new List<LineViewModel>(_order.Lines.Select(line => new LineViewModel(line, discountCalculator)));
            UpdatePrices();
        }

        public IReadOnlyCollection<LineViewModel> Lines
        {
            get
            {
                return _lines;
            }
        }

        public void UpdatePrices()
        {
            SubTotal = Lines.Sum(item => item.Amount) - _discountCalculator.GetDiscount(_order);
            Tax = _order.TaxRate * SubTotal;
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
                return SubTotal + Tax;
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

        public void AddLine(Line line)
        {
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }

            _lines.Add(new LineViewModel(line, _discountCalculator));
            _order.Lines.Add(line);
            UpdatePrices();
        }
    }
}