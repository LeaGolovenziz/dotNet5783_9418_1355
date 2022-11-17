using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using Dal;
using DalApi;
using IOrder = BlApi.IOrder;

namespace Bllmplementation
{
    internal class BlOrder : IOrder
    {
        /// <summary>
        /// access to the dal entities
        /// </summary>
        private IDal _dal = new DalList();
        Order IOrder.DeliverOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        Order IOrder.GetOrderDetails(int orderID)
        {
            throw new NotImplementedException();
        }

        IEnumerable<OrderForList> IOrder.GetOrderList()
        {
            throw new NotImplementedException();
        }

        Order IOrder.ShipOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        Order IOrder.UpdateOrderDetails(int orderID)
        {
            throw new NotImplementedException();
        }
    }
}
