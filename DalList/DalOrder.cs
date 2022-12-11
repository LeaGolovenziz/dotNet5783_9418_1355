using DalApi;
using DO;

namespace Dal;

internal class DalOrder : IOrder
{
    /// <summary>
    /// adds the order to the orders list and return it's id
    /// </summary>
    /// <param name="order"></param>
    /// <returns>int</returns>
    public int Add(Order order)
    {
        order.ID = DataSource.config.OrderID;
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
        return GetIf(order => order?.ID == id);
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
        int index = DataSource._lstOreders.FindIndex(x => x?.ID == order.ID);
        if (index == -1)
            throw new NotFound();
        DataSource._lstOreders[index] = order;
    }

    /// <summary>
    ///  return the list of orders
    /// </summary>
    /// <returns>List<Order></returns>
    public IEnumerable<Order?> Get(Func<Order?, bool>? func)
    {
        if (func != null)
            return (IEnumerable<Order?>)DataSource._lstOreders.Where(x => func(x)).ToList();
        return (IEnumerable<Order?>)DataSource._lstOreders;
    }
    /// <summary>
    ///  returns Order who meets the condition
    /// </summary>
    /// <param name="func"></param>
    /// <returns>Order</returns>
    /// <exception cref="NotFound"></exception>
    public Order GetIf(Func<Order?, bool>? func)
    {
        if (DataSource._lstOreders.Exists(x => func!(x)))
            return (Order)DataSource._lstOreders.Find(x => func!(x))!;
        throw new NotFound();
    }
}
