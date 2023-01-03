using System.ComponentModel;
using static BO.Enums;

namespace BO
{
    public class OrderTracking
    { 
        /// <summary>
        /// unique ID of the order
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// unique status of order
        /// </summary>
        public OrderStatus? OrderStatus { get; set; }

        /// <summary>
        /// unique list of tracking
        /// </summary>
        public List<Tuple<DateTime?, OrderStatus?>> Tracking { get; set; }

        public override string ToString() => this.ToStringProperty();

    }
}
