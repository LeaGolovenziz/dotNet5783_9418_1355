using System.ComponentModel;
using static BO.Enums;

namespace BO
{
    public class OrderTracking : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        private int orderID;
        /// <summary>
        /// unique ID of the order
        /// </summary>
        public int OrderID
        {
            get => orderID; 
            set
            {
                orderID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OrderID"));
            }
        }

        private OrderStatus? orderStatus;
        /// <summary>
        /// unique status of order
        /// </summary>
        public OrderStatus? OrderStatus
        {
            get => orderStatus;
            set
            {
                orderStatus = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OrderStatus"));
            }
        }


        private List<Tuple<DateTime?, OrderStatus?>> tracking;
        /// <summary>
        /// unique list of tracking
        /// </summary>
        public List<Tuple<DateTime?, OrderStatus?>> Tracking
        {
            get => tracking;
            set
            {
                tracking = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Tracking"));
            }
        }

        public override string ToString() => this.ToStringProperty();

    }
}
