using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

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
        public IEnumerable<OrderForList> GetOrderList();
        /// <summary>
        /// gets order id and returns the order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>Order</returns>
        public Order GetOrderDetails(int orderID);
        /// <summary>
        /// gets order id, update its shipping date and returns the updated order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order ShipOrder(int orderID);
        /// <summary>
        /// gets order id, update its delivery date and returns the updated order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order DeliverOrder(int orderID);
        /// <summary>
        /// gets order id, update its details and returns the updated order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order UpdateOrderDetails(int orderID, int productID, int amountToChange);

    }
}
