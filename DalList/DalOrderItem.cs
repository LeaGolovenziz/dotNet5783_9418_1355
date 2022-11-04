using DO;

namespace Dal;

public class DalOrderItem
{
    /// <summary>
    /// adds the orderItem to the orderItems list and return it's id
    /// </summary>
    /// <param name="orderItem"></param>
    /// <returns>int</returns>
    public int Add(OrderItem orderItem)
    {
        orderItem.OrderItemID = DataSource.config._ItemOrderID;
        DataSource._lstOrderItems.Add(orderItem);
        return orderItem.OrderItemID;
    }

    /// <summary>
    ///  gets an id and return the orders item with this id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>OrderItem</returns>
    /// <exception cref="Exception"></exception>
    public OrderItem Get(int id)
    {
        if (DataSource._lstOrderItems.Exists(x => x.OrderItemID == id))
            return DataSource._lstOrderItems.Find(x => x.OrderItemID == id);
        throw new Exception("the order's item doesn't exist");
    }

    /// <summary>
    /// gets a product's id and an order's id and return the orders item with this id's matches
    /// </summary>
    /// <param name="productID"></param>
    /// <param name="orderID"></param>
    /// <returns>OrderItem</returns>
    /// <exception cref="Exception"></exception>
    public OrderItem Get(int productID, int orderID)
    {
        if (DataSource._lstOrderItems.Exists(x => x.ProductID == productID && x.OrderID == orderID))
            return DataSource._lstOrderItems.Find(x => x.ProductID == productID && x.OrderID == orderID);
        throw new Exception("the order's item doesn't exist");
    }

    /// <summary>
    /// gets an ID and deletes the order's item with this ID
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        DataSource._lstOrderItems.Remove(Get(id));
    }

    /// <summary>
    /// gets an order's item and updetes it
    /// </summary>
    /// <param name="orderItem"></param>
    public void Update(OrderItem orderItem)
    {
        Delete(orderItem.OrderItemID);
        Add(orderItem);
    }

    /// <summary>
    ///  return the list of order's items
    /// </summary>
    /// <returns>List<Order></Order></returns>
    public List<OrderItem> Get()
    {
        return DataSource._lstOrderItems.ToList();
    }

    /// <summary>
    /// gets a orders id and returns the order's item of this order
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns>List<OrderItem></returns>
    public List<OrderItem> GeOrderItems(int orderID)
    {
       return DataSource._lstOrderItems.Where(x => x.OrderID == orderID).ToList();
    }
}
