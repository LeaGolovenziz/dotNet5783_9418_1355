using DalApi;
using DO;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal
{
    internal class Order : IOrder
    {
        string orderPath = @"Order.xml";

        /// <summary>
        /// Adds a new order to the file of the orders
        /// </summary>
        /// <param name="order"></param>
        /// <returns>int</returns>
        /// <exception cref="FileSavingError"></exception>
        public int Add(DO.Order order)
        {
            List<DO.Order?> orders = XmlTools.LoadListFromXMLSerializer<DO.Order?>(orderPath);

            try
            {
                Get(order.ID);
                throw new AlreadyExist();
            }
            catch
            {
                XElement configRoot = XElement.Load(XmlTools.configPath);
                int.TryParse(configRoot.Element("orderID")!.Value, out int nextSeqNum);
                order.ID = nextSeqNum;
                nextSeqNum++;
                configRoot.Element("orderID")!.SetValue(nextSeqNum);

                orders.Add(order);

                XmlTools.SaveListToXMLSerializer(orders, orderPath);
            }
            return order.ID;
        }

        /// <summary>
        /// Deletes an order from the file of orders
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete(int id)
        {
            List<DO.Order?> orders = XmlTools.LoadListFromXMLSerializer<DO.Order?>(orderPath);

            orders.Remove(Get(id));

            XmlTools.SaveListToXMLSerializer(orders, orderPath);
        }

        /// <summary>
        /// Update an order in the file of orders
        /// </summary>
        /// <param name="t"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Update(DO.Order order)
        {
            List<DO.Order?> orders = XmlTools.LoadListFromXMLSerializer<DO.Order?>(orderPath);

            orders.Remove(Get(order.ID));
            orders.Add(order);

            XmlTools.SaveListToXMLSerializer(orders, orderPath);
        }

        /// <summary>
        /// Returns list of all the orders, if gets a condition - by it
        /// </summary>
        /// <param name="func"></param>
        /// <returns>IEnumerable<DO.Order?></returns>
        public IEnumerable<DO.Order?> Get(Func<DO.Order?, bool>? func = null)
        {
            List<DO.Order?> orders = XmlTools.LoadListFromXMLSerializer<DO.Order?>(orderPath);

            return func == null ? orders : orders.Where(func);
        }

        /// <summary>
        /// gets an ID and returns the order with this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DO.Order</returns>
        /// <exception cref="nullvalue"></exception>
        public DO.Order Get(int id)
        {
            return GetIf(order => (order ?? throw new Nullvalue()).ID == id);
        }

        /// <summary>
        /// Gets a condition and returns an order with this condition
        /// </summary>
        /// <param name="func"></param>
        /// <returns>DO.Order</returns>
        /// <exception cref="NotFound"></exception>
        public DO.Order GetIf(Func<DO.Order?, bool>? func)
        {
            List<DO.Order?> orders = XmlTools.LoadListFromXMLSerializer<DO.Order?>(orderPath);

            return orders.FirstOrDefault(func!) ?? throw new NotFound();
        }
    }
}
