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

namespace Bllmplementation
{
    internal class BlOrder : IOrder
    {
        /// <summary>
        /// access to the dal entities
        /// </summary>
        private IDal _dal = new DalList();
        private Order copyOrderFromDal(ref DO.Order dalOrder, int orderID)
        {
            // total price for order
            double totalPrice = 0;
            Order blOrder = new Order();

            blOrder.ID = dalOrder.ID;
            blOrder.CustumerName = dalOrder.CustomerName;
            blOrder.CustumerEmail = dalOrder.CustomerEmail;
            blOrder.CustumerAdress = dalOrder.CustomerAdress;
            blOrder.OrderDate = dalOrder.OrderDate;
            blOrder.ShipDate = dalOrder.ShipDate;
            blOrder.DeliveryDate = dalOrder.DeliveryDate;

            // the status of order
            if (dalOrder.DeliveryDate > DateTime.Now)
                blOrder.OrderStatus = Enums.OrderStatus.Delivered;
            else if (dalOrder.ShipDate > DateTime.Now)
                blOrder.OrderStatus = Enums.OrderStatus.Sent;
            else
                blOrder.OrderStatus = Enums.OrderStatus.Confirmed;

            // The orderItems of order
            IEnumerable<OrderItem> orderItems = new List<OrderItem>();
            IEnumerable<DO.OrderItem> tempOrderItems = _dal.OrderItem.GeOrderItems(orderID);
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

        Order IOrder.DeliverOrder(int orderID)
=======

        public BO.Order DeliverOrder(int orderID)
>>>>>>> 816807b8bf99cac0cb0bcfd5252de15d332b953d
        {
            try
            {
                DO.Order dalOrder = _dal.Order.Get(orderID);
                if (dalOrder.ShipDate == null)
                {
                    dalOrder.DeliveryDate = DateTime.Now;
                    _dal.Order.Update(dalOrder);
                    dalOrder = _dal.Order.Get(orderID);
                }
                return copyOrderFromDal(ref dalOrder, orderID);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        public BO.Order GetOrderDetails(int orderID)
        {
            if (orderID <= 0)
                throw new UnvalidID();
            try
            {
                DO.Order dalOrder = _dal.Order.Get(orderID);
                return copyOrderFromDal(ref dalOrder, orderID);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        public BO.Order ShipOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        public OrderTracking TrackOrder(int orderID)
        {
            try
            {
                DO.Order order=_dal.Order.Get(orderID);

                OrderTracking orderTracking = new OrderTracking();

                orderTracking.OrderID= orderID;
                if (order.DeliveryDate < DateTime.Now)
                    orderTracking.OrderStatus = BO.Enums.OrderStatus.Delivered;
                else if (order.ShipDate < DateTime.Now)
                    orderTracking.OrderStatus = BO.Enums.OrderStatus.Sent;
                else
                    orderTracking.OrderStatus = BO.Enums.OrderStatus.Confirmed;
                orderTracking.Tracking = new List<Tuple<DateTime, BO.Enums.OrderStatus>>();
                orderTracking.Tracking.Add(new Tuple<DateTime, BO.Enums.OrderStatus>((DateTime)order.OrderDate, BO.Enums.OrderStatus.Confirmed));
                orderTracking.Tracking.Add(new Tuple<DateTime, BO.Enums.OrderStatus>((DateTime)order.ShipDate, BO.Enums.OrderStatus.Sent));
                orderTracking.Tracking.Add(new Tuple<DateTime, BO.Enums.OrderStatus>((DateTime)order.DeliveryDate, BO.Enums.OrderStatus.Delivered));

                return orderTracking;
            }
            catch(NotFound e)
            {
                throw new DoesntExist();
            }
        }

        public BO.Order UpdateOrderDetails(int orderID)
        {
            throw new NotImplementedException();
        }


        IEnumerable<OrderForList> IOrder.GetOrderList()
        {
            IEnumerable<OrderForList> orders = new List<OrderForList>();
            IEnumerable<DO.Order> temp = _dal.Order.Get();
            IEnumerable<DO.OrderItem> orderItems = _dal.OrderItem.Get();

            foreach (DO.Order order in temp)
            {
                OrderForList tempOrderForList = new OrderForList();
                tempOrderForList.OrderID = order.ID;
                tempOrderForList.CostumerName = order.CustomerName;
                if (order.DeliveryDate > DateTime.Now)
                    tempOrderForList.OrderStatus = Enums.OrderStatus.Delivered;
                else if (order.ShipDate > DateTime.Now)
                    tempOrderForList.OrderStatus = Enums.OrderStatus.Sent;
                else
                    tempOrderForList.OrderStatus = Enums.OrderStatus.Confirmed;
                tempOrderForList.Amount = orderItems.First(x => x.OrderID == order.ID).Amount;
                tempOrderForList.Price = orderItems.First(x => x.OrderID == order.ID).Price;
            }
            return orders;
        }

<<<<<<< HEAD
        Order IOrder.ShipOrder(int orderID)
        {
            try
            {
                DO.Order dalOrder = _dal.Order.Get(orderID);
                if (dalOrder.ShipDate == null)
                {
                    dalOrder.ShipDate = DateTime.Now;
                    _dal.Order.Update(dalOrder);
                    dalOrder = _dal.Order.Get(orderID);
                }
                return copyOrderFromDal(ref dalOrder, orderID);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        Order IOrder.UpdateOrderDetails(int orderID, int productID, int amountToChange)
        {
            DO.Order dalOrder = _dal.Order.Get(orderID);

            // if the order has not been sent yet
            if (dalOrder.ShipDate == null)
            {
                try
                {
                    if (amountToChange < 0)
                        throw new UnvalAmount();

                    DO.Product dalProduct = _dal.Product.Get(productID);
                    DO.OrderItem dalOrderItem = _dal.OrderItem.Get(orderID);

                    if ((dalProduct.InStock + dalOrderItem.Amount - amountToChange) < 0)
                        throw new ProductNotInStock();

                    // update the amount of the item in the order
                    dalOrderItem.Amount = amountToChange;
                    _dal.OrderItem.Update(dalOrderItem);

                    // update the amount in stock of the product
                    dalProduct.InStock += (dalOrderItem.Amount - amountToChange);
                    _dal.Product.Update(dalProduct);

                    // copy the orders and return the bl's one
                    return copyOrderFromDal(ref dalOrder, orderID);
                }
                catch (NotFound ex)
                {
                    throw new DoesntExist(ex);
                }
            }
            else
                throw new AlreadyShipped();
        }
=======
>>>>>>> 816807b8bf99cac0cb0bcfd5252de15d332b953d
    }
}
