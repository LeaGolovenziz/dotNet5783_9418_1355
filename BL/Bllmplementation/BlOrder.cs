using BO;
using DalApi;
using DO;
using System.Runtime.CompilerServices;
using IOrder = BlApi.IOrder;
using Nullvalue = BO.Nullvalue;
using Order = BO.Order;
using OrderItem = BO.OrderItem;

namespace Bllmplementation
{

    internal class BlOrder : IOrder
    {
        /// <summary>
        /// access to the dal entities
        /// </summary>
        private IDal? dal = DalApi.Factory.Get();

        /// <summary>
        /// gets dal order and ID of order and copies it to a new bl order and returns it
        /// </summary>
        /// <param name="dalOrder"></param>
        /// <param name="orderID"></param>
        /// <returns>Order</returns>

        private Order copyOrderFromDal(DO.Order? dalOrder)
        {

            Order blOrder = dalOrder.CopyPropTo(new Order());
            blOrder.OrderItems = new List<OrderItem>();

            // the status of order
            blOrder.OrderStatus = (BO.Enums.OrderStatus)getOrderStatus(dalOrder)!;
            int orderPrice = 0;

            // The orderItems of dal order
            IEnumerable<DO.OrderItem?> tempOrderItems = dal.OrderItem.GeOrderItems((int)dalOrder?.ID);
            // copy the order items list
            foreach (DO.OrderItem? item in tempOrderItems)
            {
                OrderItem tempOrderItem = item.CopyPropTo(new OrderItem());
                // calculate total price of the order item as product price * product ampount
                tempOrderItem.TotalPrice = ((item ?? throw new Nullvalue()).Price ?? throw new Nullvalue()) * ((item ?? throw new Nullvalue()).ProductAmount ?? throw new Nullvalue());
                tempOrderItem.Name = dal.Product.Get((int)item?.ID).Name;

                blOrder.OrderItems.Add(tempOrderItem);

                orderPrice += (int)tempOrderItem.TotalPrice;
            }

            // adding total price of the order item to the total price of the order
            blOrder.Price = orderPrice;

            return blOrder;
        }

        /// <summary>
        ///  gets an order and retuens its status
        /// </summary>
        /// <param name="order"></param>
        /// <returns>BO.Enums.OrderStatus?</returns>
        /// <exception cref="nullvalue"></exception>
        BO.Enums.OrderStatus? getOrderStatus(DO.Order? order)
        {
            BO.Enums.OrderStatus? orderStatus;
            // the status
            if ((order ?? throw new Nullvalue()).DeliveryDate != null && (order ?? throw new Nullvalue()).DeliveryDate <= DateTime.Now)
                orderStatus = BO.Enums.OrderStatus.Delivered;
            else if ((order ?? throw new Nullvalue()).ShipDate != null && (order ?? throw new Nullvalue()).ShipDate <= DateTime.Now)
                orderStatus = BO.Enums.OrderStatus.Sent;
            else
                orderStatus = BO.Enums.OrderStatus.Confirmed;
            return orderStatus;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Order DeliverOrder(int orderID)
        {
            try
            {
                DO.Order dalOrder = dal.Order.Get(orderID);

                // if the the order wasn't shipped yet or already delivered - throe an exception
                if (dalOrder.ShipDate == null)
                    throw new wasntShipped();
                if (dalOrder.DeliveryDate != null)
                    throw new AlreadyDelivered();

                // update the delivery date to now
                dalOrder.DeliveryDate = DateTime.Now;

                // update the order in dal
                dal.Order.Update(dalOrder);

                // copy the dal order to bl order
                return copyOrderFromDal(dalOrder);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Order GetOrderDetails(int orderID)
        {
            // if the order's ID is unvalid - throw an exception
            if (orderID <= 0)
                throw new UnvalidID();
            try
            {
                // get the order from dal
                DO.Order dalOrder = dal.Order.Get(orderID);
                // copy the dal order to bl order and return it
                return copyOrderFromDal(dalOrder);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public OrderTracking TrackOrder(int orderID)
        {
            try
            {
                // get the order from dal
                DO.Order order = dal.Order.Get(orderID);

                OrderTracking orderTracking = new OrderTracking();

                // the details of the tracking order
                // the ID
                orderTracking.OrderID = orderID;
                // the status
                orderTracking.OrderStatus = (BO.Enums.OrderStatus)getOrderStatus(order)!;
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
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<OrderForList?> IOrder.GetOrderList()
        {
            IEnumerable<OrderForList> orders;
            IEnumerable<DO.Order?> dalOrders = dal.Order.Get();
            IEnumerable<DO.OrderItem?> orderItems = dal.OrderItem.Get();

            // copy all the orders from dal to bl
            orders = dalOrders.Select(order => new OrderForList
            {
                // the ID the
                OrderID = (order ?? throw new Nullvalue()).ID!,
                // the customer's name
                CustomerName = (order ?? throw new Nullvalue()).CustomerName!,
                OrderStatus = getOrderStatus(order),
                // the amount
                Amount = orderItems.Where(x => (x ?? throw new Nullvalue()).OrderID == (order ?? throw new Nullvalue()).ID).Sum(x => (x ?? throw new Nullvalue()).ProductAmount),
                // the price
                Price = orderItems.Where(x => (x ?? throw new Nullvalue()).OrderID == (order ?? throw new Nullvalue()).ID).Sum(x => (x ?? throw new Nullvalue()).Price * (x ?? throw new Nullvalue()).ProductAmount)
            }); ;

            return orders;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        Order IOrder.ShipOrder(int orderID)
        {
            try
            {
                // get the order from dal
                DO.Order dalOrder = dal.Order.Get(orderID);

                // if the order was shipped alredy - throw an exception
                if (dalOrder.ShipDate != null)
                    throw new AlreadyShipped();

                // update the ship date to now ( - ship the order)
                dalOrder.ShipDate = DateTime.Now;

                // update the order (the ship date) of dal
                dal.Order.Update(dalOrder);

                // copy the order of dal to order of bl
                return copyOrderFromDal(dalOrder);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        Order IOrder.UpdateOrderDetails(int orderID, int productID, int amountToChange)
        {
            try
            {
                // get the order of dal
                DO.Order dalOrder = dal.Order.Get(orderID);

                // if the order has not been sent yet - update the details
                if (dalOrder.ShipDate == null)
                {

                    // if the new amount is unvaild - throw an exception
                    if (amountToChange < 0)
                        throw new UnvalidAmount();

                    // get the product and the order item from dal
                    DO.Product dalProduct = dal.Product.Get(productID);
                    DO.OrderItem dalOrderItem = dal.OrderItem.Get(productID, orderID);

                    // if there is not anough in stock - throw an exception
                    if ((dalProduct.InStock + dalOrderItem.ProductAmount - amountToChange) < 0)
                        throw new ProductNotInStock();

                    // update the amount of the order item
                    dalOrderItem.ProductAmount = amountToChange;
                    // update the order item in dal
                    dal.OrderItem.Update(dalOrderItem);

                    // update the amount in stock of the product
                    dalProduct.InStock += (dalOrderItem.ProductAmount - amountToChange);
                    // update the product in dal
                    dal.Product.Update(dalProduct);

                    // copy the orders and return the bl's one
                    return copyOrderFromDal(dalOrder);
                }
                else
                    throw new AlreadyShipped();
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public OrderForList GetOrderForList(int orderID)
        {
            if (orderID <= 0)
                throw new UnvalidID();
            try
            {
                // get the order from dal and copy the properties with the same name
                DO.Order dalOrder = dal.Order.Get(orderID);
                OrderForList orderForList = dalOrder.CopyPropTo(new OrderForList());
                // copy the rest of the properties
                orderForList.OrderID = dalOrder.ID;
                orderForList.OrderStatus = (BO.Enums.OrderStatus)getOrderStatus(dalOrder)!;
                orderForList.Amount = dal.OrderItem.GeOrderItems(orderID).Sum(orderItem => (orderItem ?? throw new Nullvalue()).ProductAmount);
                orderForList.Price = dal.OrderItem.GeOrderItems(orderID).Where(orderItem => (orderItem ?? throw new Nullvalue()).OrderID == orderID).Sum(orderItem => (orderItem ?? throw new Nullvalue()).Price * (orderItem ?? throw new Nullvalue()).ProductAmount);
                return orderForList;
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Order AddNewOrderItem(int orderID, int productID)
        {
            try
            {
                DO.Product product = dal.Product.Get(productID);
                if (product.InStock == 0)
                    throw new ProductNotInStock();

                int? newAmount;
                try
                {
                    DO.OrderItem orderItem = dal.OrderItem.Get(productID, orderID);
                    orderItem.ProductAmount++;
                    dal.OrderItem.Update(orderItem);

                }
                catch (NotFound ex)
                {
                    DO.OrderItem dalOrderItem = new DO.OrderItem
                    {
                        ID = productID,
                        OrderID = orderID,
                        Price = product.Price,
                        ProductAmount = 1
                    };

                    dal.OrderItem.Add(dalOrderItem);
                }

                return GetOrderDetails(orderID);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
            }

        }
    }
}
