﻿using static BO.Enums;

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
        /// Unique image of product
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// returns a string of the product's list details
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.ToStringProperty();
    }
}
