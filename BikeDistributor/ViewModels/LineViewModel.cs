using System;
using BikeDistributor.Models;

namespace BikeDistributor.ViewModels
{
    public class LineViewModel
    {
        private readonly Line _line;
        private readonly IDiscountCalculator<LineViewModel> _lineDiscountCalculator;

        public LineViewModel(Line line, IDiscountCalculator<LineViewModel> lineDiscountCalculator)
        {
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }

            if (line.Quantity <= 0)
            {
                throw new ArgumentException("Invalid quantity");
            }

            if (line.Bike.Price <= 0)
            {
                throw new ArgumentException("Invalid price");
            }

            _line = line;
            _lineDiscountCalculator = lineDiscountCalculator;
        }

        public int Quantity
        {
            get
            {
                return _line.Quantity;
            }
        }

        public string Model
        {
            get
            {
                return _line.Bike.Model;
            }
        }

        public string Brand
        {
            get
            {
                return _line.Bike.Brand;
            }
        }

        public decimal Price
        {
            get
            {
                return _line.Bike.Price;
            }
        }

        public decimal Discount
        {
            get
            {
                return _lineDiscountCalculator != null ? _lineDiscountCalculator.GetDiscount(this) : 0;
            }
        }

        public decimal SubTotal
        {
            get
            {
                return Quantity * Price;
            }
        }

        public decimal Total
        {
            get
            {
                return SubTotal - Discount;
            }
        }
    }
}