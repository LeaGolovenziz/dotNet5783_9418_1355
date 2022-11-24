using System;
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

        public BO.Order DeliverOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        public BO.Order GetOrderDetails(int orderID)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

    }
}
