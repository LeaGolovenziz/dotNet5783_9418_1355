using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.Enums;

namespace BO
{
    public class ProductForList
    {
        /// <summary>
        /// unique ID of pruduct's list
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// unique product's name
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// unique product's price
        /// </summary>
        public double? Price { get; set; }
        /// <summary>
        /// unique product's category
        /// </summary>
        public Category? Category { get; set; }
        /// <summary>
        /// returns a string of the product's list details
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $@"
        Product for list ID - {ID}
        Product's name - {Name} 
                  price: {Price}
                  category - {Category}
";
    }
}
