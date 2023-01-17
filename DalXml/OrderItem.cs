using DalApi;
using DO;
using System.Xml.Linq;

namespace Dal
{
    internal class OrderItem : IOrderItem
    {
        string rootName = "ordersItem";
        string orderItemPath = @"OrderItem.xml";

        /// <summary>
        /// Adds a new orderItem to the file of the orderItems
        /// </summary>
        /// <param name="t"></param>
        /// <returns>int</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Add(DO.OrderItem orderItem)
        {
            List<DO.OrderItem?> orderItems = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath, rootName);

            try
            {
                Get(orderItem.ID);
                throw new AlreadyExist();
            }
            catch(NotFound)
            {
                XElement configRoot = XElement.Load(XmlTools.configPath);
                int nextSeqNum = (int)configRoot.Element("orderItemID")!;
                orderItem.OrderItemID = nextSeqNum;
                nextSeqNum++;
                configRoot.Element("orderItemID")!.SetValue(nextSeqNum);
                configRoot.Save(XmlTools.configPath);

                orderItems.Add(orderItem);

                XmlTools.SaveListToXMLSerializer(orderItems, orderItemPath);
            }
            return orderItem.ID;
        }

        /// <summary>
        /// Deletes an orderItem from the file of the orderItems
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            List<DO.OrderItem?> orderItems = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath, rootName);

            orderItems.Remove(Get(id));

            XmlTools.SaveListToXMLSerializer(orderItems, orderItemPath);
        }

        /// <summary>
        /// Updates an order Item in the file of the orderItems
        /// </summary>
        /// <param name="t"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Update(DO.OrderItem orderItem)
        {
            List<DO.OrderItem?> orderItems = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath, rootName);

            orderItems.Remove(Get(orderItem.OrderItemID));
            orderItems.Add(orderItem);

            XmlTools.SaveListToXMLSerializer(orderItems, orderItemPath);
        }

        /// <summary>
        /// Gets an order ID and returns all the orderItems with this ID of order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns>IEnumerable<DO.OrderItem?></returns>
        /// <exception cref="nullvalue"></exception>
        public IEnumerable<DO.OrderItem?> GeOrderItems(int orderID)
        {
            List<DO.OrderItem?> orderItems = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath, rootName);

            return orderItems.Where(orderItem => (orderItem ?? throw new Nullvalue()).OrderID == orderID);
        }

        /// <summary>
        /// Gets an order and product ID and returns the matching orderItem
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="orderID"></param>
        /// <returns>DO.OrderItem</returns>
        /// <exception cref="nullvalue"></exception>
        /// <exception cref="NotFound"></exception>
        public DO.OrderItem Get(int productID, int orderID)
        {
            List<DO.OrderItem?> orderItems = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath, rootName);

            return orderItems.FirstOrDefault(orderItem => (orderItem ?? throw new Nullvalue()).OrderID == orderID && (orderItem ?? throw new Nullvalue()).ID == productID) ?? throw new NotFound();
        }

        /// <summary>
        /// Returns list of all the orderItems, if gets a condition - by it
        /// </summary>
        /// <param name="func"></param>
        /// <returns>IEnumerable<DO.OrderItem?></returns>
        public IEnumerable<DO.OrderItem?> Get(Func<DO.OrderItem?, bool>? func = null)
        {
            List<DO.OrderItem?> orderItems = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath, rootName);

            return func == null ? orderItems : orderItems.Where(func);
        }

        /// <summary>
        /// Gets an orderItem's ID and returns the orderItem with this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DO.OrderItem</returns>
        /// <exception cref="nullvalue"></exception>
        public DO.OrderItem Get(int id)
        {
            return GetIf(OrderItem => (OrderItem ?? throw new Nullvalue()).OrderItemID == id);
        }

        /// <summary>
        /// Gets a condition and returns the orderItem with this condition
        /// </summary>
        /// <param name="func"></param>
        /// <returns>DO.OrderItem</returns>
        /// <exception cref="NotFound"></exception>
        public DO.OrderItem GetIf(Func<DO.OrderItem?, bool>? func)
        {
            List<DO.OrderItem?> orderItems = XmlTools.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath, rootName);

            return orderItems.FirstOrDefault(func!) ?? throw new NotFound();
        }
    }
}


