﻿using BO;

namespace BlApi
{
    /// <summary>
    /// interface for order methods
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// returns a list of the orders
        /// </summary>
        /// <returns>IEnumerable<OrderForList></returns>
        public IEnumerable<OrderForList?> GetOrderList();
        /// <summary>
        /// gets order id and returns the Order with this ID
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>Order</returns>
        public Order GetOrderDetails(int orderID);

        /// <summary>
        /// gets order id and returns the orderForList with this ID
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>OrderForList</returns>
        public OrderForList GetOrderForList(int orderID);

        /// <summary>
        /// gets order id, update its shipping date and returns the updated order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>Order</returns>
        public Order ShipOrder(int orderID);

        /// <summary>
        /// gets order id, update its delivery date and returns the updated order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order DeliverOrder(int orderID);
        /// <summary>
        /// gets order id, and returns order tracking of that order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>OrderTracking</returns>
        public OrderTracking TrackOrder(int orderID);
        /// <summary>
        /// gets order id, update its details and returns the updated order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order UpdateOrderDetails(int orderID, int productID, int amountToChange);

        /// <summary>
        /// gets ID of order and product and adds the product to the order
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns>Order</returns>
        public Order AddNewOrderItem(int orderID, int productID);
    }
}
