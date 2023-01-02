using static BO.Enums;

namespace BO
{
    public class OrderForList
    {
        /// <summary>
        /// unique ID of orders' list
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// unique name of costumer
        /// </summary>
        public string? CustomerName { get; set; }
        /// <summary>
        /// unique name of costumer
        /// </summary>
        public OrderStatus? OrderStatus { get; set; }
        /// <summary>
        /// unique amount of products in order
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
        public override string ToString() => this.ToStringProperty();
    }
}
