using System;

namespace BikeDistributor.Models
{
    public class Bike
    {
        /// <summary>
        /// Default constructor for EF
        /// </summary>
        public Bike()
        {

        }

        public Bike(string brand, string model, decimal price)
        {
            Id = Guid.NewGuid();
            Brand = brand;
            Model = model;
            Price = price;
        }

        public Guid Id
        {
            get;
            set;
        }

        public string Brand
        {
            get;
            set;
        }

        public string Model
        {
            get;
            set;
        }

        public decimal Price
        {
            get;
            set;
        }
    }
}
