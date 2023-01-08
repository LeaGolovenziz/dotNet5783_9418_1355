using DalApi;
using DO;
using System.IO;
using System.Xml.Serialization;

namespace Dal
{
    internal class Order : IOrder
    {
        string orderPath=@"Order.xml";
        
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

                orders.Add(order);
                // Try to save the file with the additional product
                try
                {
                    XmlTools.SaveListToXMLSerializer(orders,orderPath);
                }
                catch
                {
                    throw new FileSavingError();
                }
            }
            return order.ID;
        }



        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DO.Order?> Get(Func<DO.Order?, bool>? func = null)
        {
            throw new NotImplementedException();
        }

        public DO.Order Get(int id)
        {
            throw new NotImplementedException();
        }

        public DO.Order GetIf(Func<DO.Order?, bool>? func)
        {
            throw new NotImplementedException();
        }

        public void Update(DO.Order t)
        {
            throw new NotImplementedException();
        }
    }
}
