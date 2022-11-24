using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DO.Enums;
using System.Xml.Linq;

namespace BO
{
    public class Cart
    {
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
        public List<OrderItem>? OrderItems { get; set; }
        /// <summary>
        /// unique price of products in cart
        /// </summary>
        public double? price { get; set; }
        /// <summary>
        /// returns a string of the cart's details
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $@"
        Custumer's name: {CustomerName} 
                   Email: {CustomerEmail} 
                   Address: {CustomerAddress}
        list of items: {string.Join('\n', OrderItems)}
    	Total price: {price}
";

    }
}
