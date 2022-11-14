using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BO.Enums;

namespace BO
{
    public class OrderTracking
    {
        /// <summary>
        /// unique ID of the order
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// unique status of order
        /// </summary>
        public OrderStatus? OrderStatus { get; set; }
        /// <summary>
        /// unique list of tracking
        /// </summary>
        public Tuple<DateTime,OrderStatus>? Tracking { get; set; }
        /// <summary>
        /// returns a string of the order's tracking
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $@"
        Order tracking ID - {ID}:
        Order's status: {OrderStatus} 
        tracking: {Tracking}
";
    }
}
