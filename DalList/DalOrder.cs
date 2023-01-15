using DalApi;
using DO;
using System.Runtime.CompilerServices;

namespace Dal;

public class DalOrder : IOrder
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// adds the order to the orders list and return it's id
    /// </summary>
    /// <param name="order"></param>
    /// <returns>int</returns>
    public int Add(Order order)
    {
        order.ID = DataSource.config.OrderID;
        DataSource.lstOreders.Add(order);
        return order.ID;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    ///  gets an id and return the order with this id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Order</returns>
    /// <exception cref="Exception"></exception>
    public Order Get(int id)
    {
        return GetIf(order => (order ?? throw new Nullvalue()).ID == id);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// gets an ID and deletes the order with this ID
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        DataSource.lstOreders.Remove(Get(id));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// gets an order and updetes it
    /// </summary>
    /// <param name="order"></param>
    public void Update(Order order)
    {
        int index = DataSource.lstOreders.FindIndex(x => (x ?? throw new Nullvalue()).ID == order.ID);
        if (index == -1)
            throw new NotFound();
        DataSource.lstOreders[index] = order;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    ///  return the list of orders
    /// </summary>
    /// <returns>List<Order></returns>
    public IEnumerable<Order?> Get(Func<Order?, bool>? func = null)
    {
        if (func != null)
            return (from item in DataSource.lstOreders
                    where func(item)
                    select item).ToList();
        return DataSource.lstOreders;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    ///  returns Order who meets the condition
    /// </summary>
    /// <param name="func"></param>
    /// <returns>Order</returns>
    /// <exception cref="NotFound"></exception>
    public Order GetIf(Func<Order?, bool>? func)
    {
        if (DataSource.lstOreders.Exists(x => func!(x)))
            return (Order)DataSource.lstOreders.Find(x => func!(x))!;
        throw new NotFound();
    }
}
