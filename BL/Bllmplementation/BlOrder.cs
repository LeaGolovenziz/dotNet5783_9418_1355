using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using Dal;
using DalApi;
using DO;
using IOrder = BlApi.IOrder;
using Order = BO.Order;
using OrderItem = BO.OrderItem;

namespace Bllmplementation
{
    internal class BlOrder : IOrder
    {
        /// <summary>
        /// access to the dal entities
        /// </summary>
        private IDal _dal = new DalList();
        /// <summary>
        /// gets dal order and ID of order and copies it to a new bl order and returns it
        /// </summary>
        /// <param name="dalOrder"></param>
        /// <param name="orderID"></param>
        /// <returns>Order</returns>
        private Order copyOrderFromDal(ref DO.Order dalOrder, int orderID)
        {
            // total price for order
            double totalPrice = 0;

            Order blOrder = new Order();
            blOrder.OrderItems = new List<OrderItem>();

            blOrder.ID = dalOrder.ID;
            blOrder.CustumerName = dalOrder.CustomerName;
            blOrder.CustumerEmail = dalOrder.CustomerEmail;
            blOrder.CustumerAdress = dalOrder.CustomerAdress;
            blOrder.OrderDate = dalOrder.OrderDate;
            blOrder.ShipDate = dalOrder.ShipDate;
            blOrder.DeliveryDate = dalOrder.DeliveryDate;

            // the status of order
            if (dalOrder.DeliveryDate!=null&&dalOrder.DeliveryDate <= DateTime.Now)
                blOrder.OrderStatus = BO.Enums.OrderStatus.Delivered;
            else if (dalOrder.ShipDate != null&&dalOrder.ShipDate <= DateTime.Now)
                blOrder.OrderStatus = BO.Enums.OrderStatus.Sent;
            else
                blOrder.OrderStatus = BO.Enums.OrderStatus.Confirmed;

            // The orderItems of dal order
            IEnumerable<DO.OrderItem?> tempOrderItems = _dal.OrderItem.GeOrderItems(orderID);
            // copy the order items list
            foreach (DO.OrderItem item in tempOrderItems)
            {
                OrderItem tempOrderItem = new OrderItem();
                tempOrderItem.OrderID = item.OrderID;
                tempOrderItem.ProductID = item.ProductID;
                tempOrderItem.ProductName = _dal.Product.Get(item.ProductID).Name;
                tempOrderItem.ProductPrice = item.Price;
                tempOrderItem.ProductAmount = item.Amount;
                tempOrderItem.TotalPrice = item.Price * item.Amount;

                blOrder.OrderItems.Add(tempOrderItem);

                totalPrice += (double)(item.Price * item.Amount);
            }

            blOrder.Price = totalPrice;

            return blOrder;
        }

        public Order DeliverOrder(int orderID)
        {
            try
            {
                DO.Order dalOrder = _dal.Order.Get(orderID);

                // if the the order wasn't shipped yet or already delivered - throe an exception
                if (dalOrder.ShipDate == null)
                    throw new wasntShipped();
                if (dalOrder.DeliveryDate != null)
                    throw new AlreadyDelivered();

                // update the delivery date to now
                dalOrder.DeliveryDate = DateTime.Now;

                // update the order in dal
                _dal.Order.Update(dalOrder);

                // copy the dal order to bl order
                return copyOrderFromDal(ref dalOrder, orderID);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        public BO.Order GetOrderDetails(int orderID)
        {
            // if the order's ID is unvalid - throw an exception
            if (orderID <= 0)
                throw new UnvalidID();
            try
            {
                // get the order from dal
                DO.Order dalOrder = _dal.Order.Get(orderID);
                // copy the dal order to bl order and return it
                return copyOrderFromDal(ref dalOrder, orderID);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        public OrderTracking TrackOrder(int orderID)
        {
            try
            {
                // get the order from dal
                DO.Order order = _dal.Order.Get(orderID);

                OrderTracking orderTracking = new OrderTracking();

                // the details of the tracking order
                // the ID
                orderTracking.OrderID = orderID;
                // the status
                if (order.DeliveryDate != null&&order.DeliveryDate <= DateTime.Now)
                    orderTracking.OrderStatus = BO.Enums.OrderStatus.Delivered;
                else if (order.ShipDate != null&&order.ShipDate <= DateTime.Now)
                    orderTracking.OrderStatus = BO.Enums.OrderStatus.Sent;
                else 
                    orderTracking.OrderStatus = BO.Enums.OrderStatus.Confirmed;
                // the list of tracking
                orderTracking.Tracking = new List<Tuple<DateTime?, BO.Enums.OrderStatus?>>();
                orderTracking.Tracking.Add(new Tuple<DateTime?, BO.Enums.OrderStatus?>((DateTime)order.OrderDate, BO.Enums.OrderStatus.Confirmed));
                if (order.ShipDate != null)
                    orderTracking.Tracking.Add(new Tuple<DateTime?, BO.Enums.OrderStatus?>((DateTime)order.ShipDate, BO.Enums.OrderStatus.Sent));
                if (order.DeliveryDate !=null)
                    orderTracking.Tracking.Add(new Tuple<DateTime?, BO.Enums.OrderStatus?>((DateTime)order.DeliveryDate, BO.Enums.OrderStatus.Delivered));

                return orderTracking;
            }
            catch (NotFound e)
            {
                throw new DoesntExist(e);
            }
        }

        IEnumerable<OrderForList?> IOrder.GetOrderList()
        {
            List<OrderForList> orders = new List<OrderForList>();
            IEnumerable<DO.Order?> dalOrders = _dal.Order.Get();
            IEnumerable<DO.OrderItem?> orderItems = _dal.OrderItem.Get();

            // copy all the orders from dal to bl
            foreach (DO.Order order in dalOrders)
            {
                OrderForList tempOrderForList = new OrderForList();

                // the ID the
                tempOrderForList.OrderID = order.ID;
                // the customer's name
                tempOrderForList.CostumerName = order.CustomerName;
                // the status
                if (order.DeliveryDate > DateTime.Now)
                    tempOrderForList.OrderStatus = BO.Enums.OrderStatus.Delivered;
                else if (order.ShipDate > DateTime.Now)
                    tempOrderForList.OrderStatus = BO.Enums.OrderStatus.Sent;
                else
                    tempOrderForList.OrderStatus = BO.Enums.OrderStatus.Confirmed;
                // the amount
                tempOrderForList.Amount = orderItems.First(x => x.Value.OrderID == order.ID).Value.Amount;
                // the price
                tempOrderForList.Price = orderItems.First(x => x.Value.OrderID == order.ID).Value.Price;

                // add the order to the list of the orders
                orders.Add(tempOrderForList);
            }
            return orders;
        }

        Order IOrder.ShipOrder(int orderID)
        {
            try
            {
                // get the order from dal
                DO.Order dalOrder = _dal.Order.Get(orderID);
                
                // if the order was shipped alredy - throw an exception
                if (dalOrder.ShipDate != null)
                    throw new AlreadyShipped();
                
                // update the ship date to now ( - ship the order)
                dalOrder.ShipDate = DateTime.Now;

                // update the order (the ship date) of dal
                _dal.Order.Update(dalOrder);

                // copy the order of dal to order of bl
                return copyOrderFromDal(ref dalOrder, orderID);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        Order IOrder.UpdateOrderDetails(int orderID, int productID, int amountToChange)
        {
            try
            {
                // get the order of dal
                DO.Order dalOrder = _dal.Order.Get(orderID);

            // if the order has not been sent yet - update the details
            if (dalOrder.ShipDate == null)
            {
               
                    // if the new amount is unvaild - throw an exception
                    if (amountToChange < 0)
                        throw new UnvalidAmount();

                    // get the product and the order item from dal
                    DO.Product dalProduct = _dal.Product.Get(productID);
                    DO.OrderItem dalOrderItem = _dal.OrderItem.Get(productID, orderID);

                    // if there is not anough in stock - throw an exception
                    if ((dalProduct.InStock + dalOrderItem.Amount - amountToChange) < 0)
                        throw new ProductNotInStock();

                    // update the amount and price of the order item
                    dalOrderItem.Price = amountToChange * dalProduct.Price;
                    dalOrderItem.Amount = amountToChange;
                    // update the order item in dal
                    _dal.OrderItem.Update(dalOrderItem);

                    // update the amount in stock of the product
                    dalProduct.InStock += (dalOrderItem.Amount - amountToChange);
                    // update the product in dal
                    _dal.Product.Update(dalProduct);

                    // copy the orders and return the bl's one
                    return copyOrderFromDal(ref dalOrder, orderID);
             
            }
            else
                throw new AlreadyShipped();
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }
    }
}
