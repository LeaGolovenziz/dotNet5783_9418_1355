using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.Enums;

namespace BO
{
    public class Order
    {
        /// <summary>
        /// Unique ID of order
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Unique custumer's name 
        /// </summary>
        public string? CustumerName { get; set; }
        /// <summary>
        /// Unique austumer's email
        /// </summary>
        public string? CustumerEmail { get; set; }
        /// <summary>
        /// Unique custumer's adress
        /// </summary>
        public string? CustumerAdress { get; set; }
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
        public List<OrderItem>? OrderItems { get; set; }
        /// <summary>
        /// unique total price of order
        /// </summary>
        public double? Price { get; set; } 
        /// <summary>
        /// returns a string of the order's details
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => $@"
        Order's ID - {ID}: 

        Custumer's name: {CustumerName}
                   Email: {CustumerEmail}
                   adress: {CustumerAdress}

        Order's status: {OrderStatus}
               order date - {OrderDate}
               ship date - {ShipDate}
               delivery date - {DeliveryDate}

        List of items: 
                 {string.Join('\n', OrderItems)}

        Total price: {Price}
";
    }
}
