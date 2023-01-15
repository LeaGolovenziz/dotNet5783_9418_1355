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
        /// Unique custumer's ID of order
        /// </summary>
        public int CustomerID { get; set; }
        /// <summary>
        /// Unique austumer's email
        /// </summary>
        public string? CustomerEmail { get; set; }
        /// <summary>
        /// Unique custumer's adress
        /// </summary>
        public string? CustomerAdress { get; set; }

        private OrderStatus orderStatus;
        /// <summary>
        /// unique status of order
        /// </summary>
        public OrderStatus OrderStatus
        {
            get => orderStatus;
            set
            {
                orderStatus = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OrderStatus"));
            }
        }

        /// <summary>
        /// Unique date of order
        /// </summary>
        public DateTime? OrderDate { get; set; }

        private DateTime? shipDate;
        /// <summary>
        /// Unique ship date of order
        /// </summary>
        public DateTime? ShipDate
        {
            get => shipDate;
            set
            {
                shipDate = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ShipDate"));
            }
        }

        private DateTime? deliveryDate;
        /// <summary>
        /// Unique delivery date of order
        /// </summary>
        public DateTime? DeliveryDate
        {
            get => deliveryDate;
            set
            {
                deliveryDate = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DeliveryDate"));
            }
        }

        private List<OrderItem> orederItems;
        /// <summary>
        /// unique order's list of items
        /// </summary>
        public List<OrderItem> OrderItems
        {
            get => orederItems;
            set
            {
                orederItems = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OrderItems"));
            }
        }

        private double? price;
        /// <summary>
        /// unique total price of order
        /// </summary>
        public double? Price
        {
            get => price;
            set
            {
                price = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Price"));
            }
        }

        public override string ToString() => this.ToStringProperty();

    }
}
