using BikeDistributor.Models;

namespace BikeDistributor.ViewModels
{
    public class LineViewModel
    {
        private readonly Line _line;
        private readonly ILineDiscountStrategy _lineDiscountStrategy;

        public LineViewModel(Line line, ILineDiscountStrategy lineDiscountStrategy)
        {
            _line = line;
            _lineDiscountStrategy = lineDiscountStrategy;
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
                return _lineDiscountStrategy.GetDiscount(_line);
            }
        }

        public decimal Amount
        {
            get
            {
                return Quantity * Price - Discount;
            }
        }
    }
}