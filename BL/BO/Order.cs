using System.ComponentModel;
using static BO.Enums;

namespace BO
{
    public class Order : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Unique ID of order
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Unique custumer's name 
        /// </summary>
        public string? CustomerName { get; set; }
        /// <summary>
        /// Unique austumer's email
        /// </summary>
        public string? CustomerEmail { get; set; }
        /// <summary>
        /// Unique custumer's adress
        /// </summary>
        public string? CustomerAdress { get; set; }
        /// <summary>
        /// unique status of order
        /// </summary>
        public OrderStatus OrderStatus { get; set; }
        /// <summary>
        /// Unique date of order
        /// </summary>
        public DateTime? OrderDate { get; set; }
        /// <summary>
        /// Unique ship date of order
        /// </summary>
        public DateTime? ShipDate { get; set; }
        /// <summary>
        /// Unique delivery date of order
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
        /// <summary>
        /// unique order's list of items
        /// </summary>
        public List<OrderItem> OrderItems { get; set; }
        /// <summary>
        /// unique total price of order
        /// </summary>
        public double? Price { get; set; }

        public override string ToString() => this.ToStringProperty();

    }
}
