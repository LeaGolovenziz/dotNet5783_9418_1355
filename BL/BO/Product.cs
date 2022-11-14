using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static DO.Enums;

namespace BO
{
    public class Product
    {
        /// <summary>
        /// Unique ID of product
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Unique name of product
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Unique price of product
        /// </summary>
        public double? Price { get; set; }
        /// <summary>
        /// Unique catgory of product
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// Unique inStock of product
        /// </summary>
        public int? InStock { get; set; }

        /// <summary>
        /// returns a string of the product's details
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $@"
        Product's ID = {ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    	Amount in stock: {InStock}
";
    }
}
