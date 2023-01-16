using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Simulator
{

    static public class SimulatorC
    {
        private static BlApi.IBl bl = BlApi.Factory.Get();
        private static volatile bool stop;

        public static void runsim()
        {
            stop = false;

            DateTime dt = DateTime.Now;
            int IdOfOrderToUpdate = 0;

            // Find the order to update
            foreach (BO.OrderForList orderForList in bl.Order.GetOrderList())
            {
                if (stop)
                    break;
                BO.Order order = bl.Order.GetOrderDetails(orderForList.OrderID);

                if (order.DeliveryDate == null)
                {
                    if (order.ShipDate != null)
                    {
                        if (order.ShipDate < dt)
                        {
                            IdOfOrderToUpdate = order.ID;
                            dt = (DateTime)order.ShipDate;
                        }
                    }
                    else if (order.OrderDate != null)
                    {
                        if (order.OrderDate < dt)
                        {
                            IdOfOrderToUpdate = order.ID;
                            dt = (DateTime)order.OrderDate;
                        }
                    }
                }
            }

            if (IdOfOrderToUpdate != 0)
            {
                // Update this order
                if (bl.Order.GetOrderDetails(IdOfOrderToUpdate).ShipDate != null)
                    bl.Order.DeliverOrder(IdOfOrderToUpdate);
                else
                    bl.Order.ShipOrder(IdOfOrderToUpdate);
            }
        }

        public static void startSimulation()
        {
            Thread sim = new Thread(runsim);

            if(!sim.IsAlive)
                sim.Start();
        }

        public static void stopSimulation()
        {
            stop = true;
        }
    }
}
