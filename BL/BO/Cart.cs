using System.ComponentModel;

namespace BO
{
    public class Cart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private double? price;

        /// <summary>
        /// unique customer name 
        /// </summary>
        public string? CustomerName { get; set; }
        /// <summary>
        /// unique customer address
        /// </summary>
        public string? CustomerAddress { get; set; }
        /// <summary>
        /// unique custumer Email
        /// </summary>
        public string? CustomerEmail { get; set; }
        /// <summary>
        /// unique list of items in cart
        /// </summary>
        public List<OrderItem> OrderItems { get; set; }
        /// <summary>
        /// unique price of products in cart
        /// </summary>
        public double? Price { get => price; set { price = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Price")); } }
        /// <summary>
        /// returns a string of the cart's details
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.ToStringProperty();

    }
}
