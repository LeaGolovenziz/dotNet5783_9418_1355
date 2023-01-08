using DO;
namespace Dal;
using DalApi;

public class DalOrderItem : IOrderItem
{
    /// <summary>
    /// adds the orderItem to the orderItems list and return it's id
    /// </summary>
    /// <param name="orderItem"></param>
    /// <returns>int</returns>
    public int Add(OrderItem orderItem)
    {
        orderItem.OrderItemID = DataSource.config.ItemOrderID;
        DataSource.lstOrderItems.Add(orderItem);
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
        if (!DataSource.lstOrderItems.Exists(x => (x ?? throw new nullvalue()).OrderItemID == id))
            throw new NotFound();
        return (OrderItem)DataSource.lstOrderItems.Find(x => (x ?? throw new nullvalue()).OrderItemID == id)!;
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
        return GetIf(orderItem => (orderItem ?? throw new nullvalue()).ID == productID && (orderItem ?? throw new nullvalue()).OrderID == orderID);
    }

    /// <summary>
    /// gets an ID and deletes the order's item with this ID
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        DataSource.lstOrderItems.Remove(Get(id));
    }

    /// <summary>
    /// gets an order's item and updetes it
    /// </summary>
    /// <param name="orderItem"></param>
    public void Update(OrderItem orderItem)
    {
        int index = DataSource.lstOrderItems.FindIndex(x => (x ?? throw new nullvalue()).OrderID == orderItem.OrderID && (x ?? throw new nullvalue()).ID == orderItem.ID);
        if (index == -1)
            throw new NotFound();
        DataSource.lstOrderItems[index] = orderItem;
    }

    /// <summary>
    ///  return the list of order's items
    /// </summary>
    /// <returns>List<Order></Order></returns>
    public IEnumerable<OrderItem?> Get(Func<OrderItem?, bool>? func = null)
    {
        if (func != null)
            return (from item in DataSource.lstOrderItems
                    where func(item)
                    select item).ToList();
        return DataSource.lstOrderItems;
    }

    /// <summary>
    /// gets a orders id and returns the order's item of this order
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns>List<OrderItem></returns>
    IEnumerable<OrderItem?> IOrderItem.GeOrderItems(int orderID)
    {
        return Get(orderItem => (orderItem ?? throw new nullvalue()).OrderID == orderID);
    }
    /// <summary>
    ///  returns OrderItem who meets the condition
    /// </summary>
    /// <param name="func"></param>
    /// <returns>OrderItem</returns>
    /// <exception cref="NotFound"></exception>
    public OrderItem GetIf(Func<OrderItem?, bool>? func)
    {
        if (DataSource.lstOrderItems.Exists(x => func!(x)))
            return (OrderItem)DataSource.lstOrderItems.Find(x => func!(x))!;
        throw new NotFound();
    }


}
