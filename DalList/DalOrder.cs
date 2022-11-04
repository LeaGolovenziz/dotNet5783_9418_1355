using DO;
using System.Security.Cryptography.X509Certificates;

namespace Dal;

public class DalOrder
{
    /// <summary>
    /// adds the order to the orders list and return it's id
    /// </summary>
    /// <param name="order"></param>
    /// <returns>int</returns>
    public int Add(Order order)
    {
        order.ID = DataSource.config._OrderID;
        DataSource._lstOreders.Add(order);
        return order.ID; 
    }

    /// <summary>
    ///  gets an id and return the order with this id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Order</returns>
    /// <exception cref="Exception"></exception>
    public Order Get(int id)
    {
        if (DataSource._lstOreders.Exists(x => x.ID == id))
            return DataSource._lstOreders.Find(x=>x.ID==id);
        throw new Exception("the order doesn't exist");
    }

    /// <summary>
    /// gets an ID and deletes the order with this ID
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        DataSource._lstOreders.Remove(Get(id));
    }

    /// <summary>
    /// gets an order and updetes it
    /// </summary>
    /// <param name="order"></param>
    public void Update(Order order)
    {
        Delete(order.ID);
        Add(order);
    }

    /// <summary>
    ///  return the list of orders
    /// </summary>
    /// <returns>List<Order></returns>
    public IEnumerable<Order> Get()
    {
        return DataSource._lstOreders;
    }
}
