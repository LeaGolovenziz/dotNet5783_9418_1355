using BO;
using Dal;
using DalApi;
using System.Collections;
using System.Reflection;
using IOrder = BlApi.IOrder;
using Order = BO.Order;
using OrderItem = BO.OrderItem;

namespace Bllmplementation
{
    static class Copy
    {

        public static string ToStringProperty<Item>(this Item _item, string result = "")
        {
            IEnumerable<PropertyInfo> propertyInfos = _item!.GetType().GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                var value = propertyInfo.GetValue(_item, null);
                if (value is IEnumerable && value is not string)
                {
                    IEnumerable items = (IEnumerable)value;

                    foreach (var item in items)
                        item.ToStringProperty(result);
                }
                else
                    result += $"{propertyInfo.Name}: {value}\n";

            }
            return result;
        }

        public static Target CopyPropTo<Source, Target>(this Source source, Target target)
        {
            Dictionary<string, PropertyInfo> propertyInfoTarget = target.GetType().GetProperties()
                .ToDictionary(key => key.Name, value => value);

            IEnumerable<PropertyInfo> propertyInfoSource = source.GetType().GetProperties();

            foreach (var item in propertyInfoSource)
            {
                if (propertyInfoTarget.ContainsKey(item.Name) && (item.PropertyType == typeof(string) || !item.PropertyType.IsClass))
                {
                    Type typeSource = Nullable.GetUnderlyingType(item.PropertyType)!;
                    Type typeTarget = Nullable.GetUnderlyingType(propertyInfoTarget[item.Name].PropertyType)!;

                    object value = item.GetValue(source)!;

                    if (value is not null)
                    {
                        if (typeSource is not null && typeSource.IsEnum)
                            propertyInfoTarget[item.Name].SetValue(target, Enum.ToObject(typeTarget, value));

                        else if (propertyInfoTarget[item.Name].PropertyType == item.PropertyType)
                            propertyInfoTarget[item.Name].SetValue(target, value);
                    }
                }
            }

            return target;
        }

        public static Target CopyPropToStruct<Source, Target>(this Source source, Target target) where Target : struct
        {
            object obj = target;
            source.CopyPropTo(obj);
            return (Target)obj;
        }

        public static IEnumerable<Target> CopyListTo<Source, Target>(this IEnumerable<Source> sources) where Target : new()
        => from source in sources
           select source.CopyPropTo(new Target());

        public static IEnumerable<Target> CopyListToStruct<Source, Target>(this IEnumerable<Source> sources) where Target : struct
            => from source in sources
               select source.CopyPropTo(new Target());
    }
    internal class BlOrder : IOrder
    {
        /// <summary>
        /// access to the dal entities
        /// </summary>
        private IDal _dal = new DalList();

        /// <summary>
        /// gets dal order and ID of order and copies it to a new bl order and returns it
        /// </summary>
        /// <param name="dalOrder"></param>
        /// <param name="orderID"></param>
        /// <returns>Order</returns>

        private Order copyOrderFromDal(ref DO.Order dalOrder, int orderID)
        {
            // total price for order
            double totalPrice = 0;

            Order blOrder = dalOrder.CopyPropTo(new Order());
            blOrder.OrderItems = new List<OrderItem>();

            blOrder.ID = dalOrder.ID;
            blOrder.CustomerName = dalOrder.CustomerName;
            blOrder.CustomerEmail = dalOrder.CustomerEmail;
            blOrder.CustomerAdress = dalOrder.CustomerAdress;
            blOrder.OrderDate = dalOrder.OrderDate;
            blOrder.ShipDate = dalOrder.ShipDate;
            blOrder.DeliveryDate = dalOrder.DeliveryDate;

            // the status of order
            if (dalOrder.DeliveryDate != null && dalOrder.DeliveryDate <= DateTime.Now)
                blOrder.OrderStatus = BO.Enums.OrderStatus.Delivered;
            else if (dalOrder.ShipDate != null && dalOrder.ShipDate <= DateTime.Now)
                blOrder.OrderStatus = BO.Enums.OrderStatus.Sent;
            else
                blOrder.OrderStatus = BO.Enums.OrderStatus.Confirmed;

            // The orderItems of dal order
            IEnumerable<DO.OrderItem?> tempOrderItems = _dal.OrderItem.GeOrderItems(orderID);
            // copy the order items list
            foreach (DO.OrderItem? item in tempOrderItems)
            {
                OrderItem tempOrderItem = new OrderItem();
                tempOrderItem.OrderID = (int)item?.OrderID!;
                tempOrderItem.ProductID = (int)item?.ProductID!;
                tempOrderItem.ProductName = _dal.Product.Get((int)item?.ProductID!).Name;
                tempOrderItem.ProductPrice = (double)item?.ProductPrice!;
                tempOrderItem.ProductAmount = (int)item?.ProductAmount!;
                tempOrderItem.TotalPrice = item?.ProductPrice * item?.ProductAmount;

                blOrder.OrderItems.Add(tempOrderItem);

                totalPrice += (double)(item?.ProductPrice * item?.ProductAmount)!;
            }

            blOrder.Price = totalPrice;

            return blOrder;
        }

        public Order DeliverOrder(int orderID)
        {
            try
            {
                DO.Order dalOrder = _dal.Order.Get(orderID);

                // if the the order wasn't shipped yet or already delivered - throe an exception
                if (dalOrder.ShipDate == null)
                    throw new wasntShipped();
                if (dalOrder.DeliveryDate != null)
                    throw new AlreadyDelivered();

                // update the delivery date to now
                dalOrder.DeliveryDate = DateTime.Now;

                // update the order in dal
                _dal.Order.Update(dalOrder);

                // copy the dal order to bl order
                return copyOrderFromDal(ref dalOrder, orderID);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        public BO.Order GetOrderDetails(int orderID)
        {
            // if the order's ID is unvalid - throw an exception
            if (orderID <= 0)
                throw new UnvalidID();
            try
            {
                // get the order from dal
                DO.Order dalOrder = _dal.Order.Get(orderID);
                // copy the dal order to bl order and return it
                return copyOrderFromDal(ref dalOrder, orderID);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        public OrderTracking TrackOrder(int orderID)
        {
            try
            {
                // get the order from dal
                DO.Order order = _dal.Order.Get(orderID);

                OrderTracking orderTracking = new OrderTracking();

                // the details of the tracking order
                // the ID
                orderTracking.OrderID = orderID;
                // the status
                if (order.DeliveryDate != null && order.DeliveryDate <= DateTime.Now)
                    orderTracking.OrderStatus = BO.Enums.OrderStatus.Delivered;
                else if (order.ShipDate != null && order.ShipDate <= DateTime.Now)
                    orderTracking.OrderStatus = BO.Enums.OrderStatus.Sent;
                else
                    orderTracking.OrderStatus = BO.Enums.OrderStatus.Confirmed;
                // the list of tracking
                orderTracking.Tracking = new List<Tuple<DateTime?, BO.Enums.OrderStatus?>>();
                orderTracking.Tracking.Add(new Tuple<DateTime?, BO.Enums.OrderStatus?>((DateTime)order.OrderDate!, BO.Enums.OrderStatus.Confirmed));
                if (order.ShipDate != null)
                    orderTracking.Tracking.Add(new Tuple<DateTime?, BO.Enums.OrderStatus?>((DateTime)order.ShipDate, BO.Enums.OrderStatus.Sent));
                if (order.DeliveryDate != null)
                    orderTracking.Tracking.Add(new Tuple<DateTime?, BO.Enums.OrderStatus?>((DateTime)order.DeliveryDate, BO.Enums.OrderStatus.Delivered));

                return orderTracking;
            }
            catch (NotFound e)
            {
                throw new DoesntExist(e);
            }
        }

        IEnumerable<OrderForList?> IOrder.GetOrderList()
        {
            List<OrderForList> orders = new List<OrderForList>();
            IEnumerable<DO.Order?> dalOrders = _dal.Order.Get();
            IEnumerable<DO.OrderItem?> orderItems = _dal.OrderItem.Get();

            // copy all the orders from dal to bl
            foreach (DO.Order? order in dalOrders)
            {
                OrderForList tempOrderForList = new OrderForList();

                // the ID the
                tempOrderForList.OrderID = (int)order?.ID!;
                // the customer's name
                tempOrderForList.CostumerName = order?.CustomerName!;
                // the status
                if (order?.DeliveryDate > DateTime.Now)
                    tempOrderForList.OrderStatus = BO.Enums.OrderStatus.Delivered;
                else if (order?.ShipDate > DateTime.Now)
                    tempOrderForList.OrderStatus = BO.Enums.OrderStatus.Sent;
                else
                    tempOrderForList.OrderStatus = BO.Enums.OrderStatus.Confirmed;
                // the amount
                tempOrderForList.Amount = orderItems.FirstOrDefault(x => x?.OrderID == order?.ID)?.ProductAmount;
                // the price
                tempOrderForList.Price = orderItems.FirstOrDefault(x => x?.OrderID == order?.ID)?.ProductPrice;

                // add the order to the list of the orders
                orders.Add(tempOrderForList);
            }
            return orders;
        }

        Order IOrder.ShipOrder(int orderID)
        {
            try
            {
                // get the order from dal
                DO.Order dalOrder = _dal.Order.Get(orderID);

                // if the order was shipped alredy - throw an exception
                if (dalOrder.ShipDate != null)
                    throw new AlreadyShipped();

                // update the ship date to now ( - ship the order)
                dalOrder.ShipDate = DateTime.Now;

                // update the order (the ship date) of dal
                _dal.Order.Update(dalOrder);

                // copy the order of dal to order of bl
                return copyOrderFromDal(ref dalOrder, orderID);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        Order IOrder.UpdateOrderDetails(int orderID, int productID, int amountToChange)
        {
            try
            {
                // get the order of dal
                DO.Order dalOrder = _dal.Order.Get(orderID);

                // if the order has not been sent yet - update the details
                if (dalOrder.ShipDate == null)
                {

                    // if the new amount is unvaild - throw an exception
                    if (amountToChange < 0)
                        throw new UnvalidAmount();

                    // get the product and the order item from dal
                    DO.Product dalProduct = _dal.Product.Get(productID);
                    DO.OrderItem dalOrderItem = _dal.OrderItem.Get(productID, orderID);

                    // if there is not anough in stock - throw an exception
                    if ((dalProduct.InStock + dalOrderItem.ProductAmount - amountToChange) < 0)
                        throw new ProductNotInStock();

                    // update the amount and price of the order item
                    dalOrderItem.ProductPrice = amountToChange * dalProduct.Price;
                    dalOrderItem.ProductAmount = amountToChange;
                    // update the order item in dal
                    _dal.OrderItem.Update(dalOrderItem);

                    // update the amount in stock of the product
                    dalProduct.InStock += (dalOrderItem.ProductAmount - amountToChange);
                    // update the product in dal
                    _dal.Product.Update(dalProduct);

                    // copy the orders and return the bl's one
                    return copyOrderFromDal(ref dalOrder, orderID);

                }
                else
                    throw new AlreadyShipped();
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }
    }
}
