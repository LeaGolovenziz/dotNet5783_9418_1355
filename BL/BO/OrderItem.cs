using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class OrderItem
    {
        /// <summary>
        /// Unique ID of the Ordered Item
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// Unique Product's ID of the Ordered Item
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// Unique Product's name of the Ordered Item
        /// </summary>
        public string? ProductName { get; set; }
        /// <summary>
        /// Unique Price of the product
        /// </summary>
        public double? ProductPrice { get; set; }
        /// <summary>
        /// unique amount of products
        /// </summary>
        public int? ProductAmount { get; set; }
        /// <summary>
        /// Unique Total price of the Ordered Item
        /// </summary>
        public double? TotalPrice { get; set; }

        /// <summary>
        /// returns a string of the ordered item's details
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => $@"
       Order item ID -{OrderID}
       Product's ID - {ProductID}
                 name - {ProductName}   
                 price - {ProductPrice}
                 amount - {ProductAmount}
       Total price  {TotalPrice}
";
    }
}
