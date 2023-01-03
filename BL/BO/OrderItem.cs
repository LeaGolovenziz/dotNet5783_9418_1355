using System.ComponentModel;

namespace BO
{
    public class OrderItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int? productAmount;
        private double? totalPrice;

        /// <summary>
        /// Unique ID of the Ordered Item
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// Unique Product's ID of the Ordered Item
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Unique Product's name of the Ordered Item
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Unique Price of the product
        /// </summary>
        public double? Price { get; set; }
        
        /// <summary>
        /// unique amount of products
        /// </summary>
        public int? ProductAmount { get=>productAmount; set { productAmount = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ProductAmount")); } }
        /// <summary>
        /// Unique Total price of the Ordered Item
        /// </summary>
        public double? TotalPrice { get=>totalPrice; set { totalPrice = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("TotalPrice")); } }

        /// <summary>
        /// returns a string of the ordered item's details
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => this.ToStringProperty();

    }
}
