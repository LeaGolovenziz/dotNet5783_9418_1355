using System.ComponentModel;
using static BO.Enums;

namespace BO
{
    public class ProductItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int? amountInCart;
        /// <summary>
        /// unique ID of productItem
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// unique name of product
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// unique price of product
        /// </summary>
        public double? Price { get; set; }
        /// <summary>
        /// unique category of product
        /// </summary>
        public Category? Category { get; set; }
        /// <summary>
        /// unique is in stock flag
        /// </summary>
        public bool? InStock { get; set; }
        /// <summary>
        /// Unique image of product
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// unique amount of product in cart
        /// </summary>
        public int? AmountInCart { get => amountInCart; set { amountInCart = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("AmountInCart")); } }

        private string isInStock()
        {
            return (bool)InStock! ? "true" : "false";
        }

        /// <summary>
        /// returns a string of the ordered item's details
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => this.ToStringProperty();
    }
}
