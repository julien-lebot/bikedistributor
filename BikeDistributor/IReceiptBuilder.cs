using BikeDistributor.ViewModels;

namespace BikeDistributor
{
    /// <summary>
    /// Generates a receipt for an order.
    /// </summary>
    public interface IReceiptBuilder
    {
        /// <summary>
        /// Generates a receipt for an order.
        /// The format of the text returned depends on the implementation.
        /// </summary>
        /// <param name="order">The order to generate a receipt for.</param>
        /// <returns>A text representation of the receipt.</returns>
        string GenerateReceipt(OrderViewModel order);
    }
}