using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.Enums;

namespace BO
{
    public class OrderForList
    {
        /// <summary>
        /// unique ID of orders' list
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// unique name of costumer
        /// </summary>
        public string? CostumerName { get; set; }
        /// <summary>
        /// unique name of costumer
        /// </summary>
        public OrderStatus? OrderStatus { get; set; }
        /// <summary>
        /// unique amount of products in orer
        /// </summary>
        public int? Amount { get; set; }
        /// <summary>
        /// unique total price of order
        /// </summary>
        public double? Price { get; set; }
        /// <summary>
        /// returns a string of the order's details
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => $@"
        Order for list ID - {ID}:
        Custumer's name: {CostumerName}
        Order's status: {OrderStatus}
        Amount of items: {Amount}
        Total price: {Price}
";

    }
}
