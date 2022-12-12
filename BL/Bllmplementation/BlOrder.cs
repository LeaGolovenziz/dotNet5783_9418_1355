using BO;
using Dal;
using DalApi;
using DO;
using System.Collections;
using System.Reflection;
using IOrder = BlApi.IOrder;
using nullvalue = BO.nullvalue;
using Order = BO.Order;
using OrderItem = BO.OrderItem;

namespace Bllmplementation
{  
    
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

        private Order copyOrderFromDal(DO.Order? dalOrder, int orderID)
        {
            // total price for order
            double totalPrice = 0;

            Order blOrder = dalOrder.CopyPropTo(new Order());
            blOrder.OrderItems = new List<OrderItem>();

            // the status of order
            blOrder.OrderStatus = (BO.Enums.OrderStatus)getOrderStatus(dalOrder)!;

            // The orderItems of dal order
            IEnumerable<DO.OrderItem?> tempOrderItems = _dal.OrderItem.GeOrderItems(orderID);
            // copy the order items list
            foreach (DO.OrderItem? item in tempOrderItems)
            {
                OrderItem tempOrderItem = item.CopyPropTo(new OrderItem());
                // calculate total price of the order item as product price * product ampount
                tempOrderItem.TotalPrice =( (item ?? throw new nullvalue()).Price?? throw new nullvalue()) * ((item ?? throw new nullvalue()).ProductAmount?? throw new nullvalue());

                blOrder.OrderItems.Add(tempOrderItem);

                // adding total price of the order item to the total price of the order
                totalPrice += (double)(((item ?? throw new nullvalue()).Price ?? throw new nullvalue()) * ((item ?? throw new nullvalue()).ProductAmount?? throw new nullvalue()))!;
            }

            blOrder.Price = totalPrice;

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
            if ((order ?? throw new nullvalue()).DeliveryDate != null && (order ?? throw new nullvalue()).DeliveryDate <= DateTime.Now)
                orderStatus = BO.Enums.OrderStatus.Delivered;
            else if ((order ?? throw new nullvalue()).ShipDate != null && (order ?? throw new nullvalue()).ShipDate <= DateTime.Now)
                orderStatus = BO.Enums.OrderStatus.Sent;
            else
                orderStatus = BO.Enums.OrderStatus.Confirmed;
            return orderStatus; 
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
                return copyOrderFromDal( dalOrder, orderID);
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
                return copyOrderFromDal( dalOrder, orderID);
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
        }

        IEnumerable<OrderForList?> IOrder.GetOrderList()
        {
            IEnumerable<OrderForList> orders;
            IEnumerable<DO.Order?> dalOrders = _dal.Order.Get();
            IEnumerable<DO.OrderItem?> orderItems = _dal.OrderItem.Get();

            // copy all the orders from dal to bl
            orders = dalOrders.Select(order => new OrderForList
            {
                // the ID the
                OrderID = (order ?? throw new nullvalue()).ID!,
                // the customer's name
                CostumerName = (order ?? throw new nullvalue()).CustomerName!,
                OrderStatus = getOrderStatus(order),
                // the amount
                Amount = (orderItems.FirstOrDefault(x => (x ?? throw new nullvalue()).OrderID == (order ?? throw new nullvalue()).ID) ?? throw new nullvalue()).ProductAmount,
                // the price
                Price = (orderItems.FirstOrDefault(x => (x ?? throw new nullvalue()).OrderID == (order ?? throw new nullvalue()).ID) ?? throw new nullvalue()).Price
            });

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
                return copyOrderFromDal( dalOrder, orderID);
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
                    dalOrderItem.Price = amountToChange * dalProduct.Price;
                    dalOrderItem.ProductAmount = amountToChange;
                    // update the order item in dal
                    _dal.OrderItem.Update(dalOrderItem);

                    // update the amount in stock of the product
                    dalProduct.InStock += (dalOrderItem.ProductAmount - amountToChange);
                    // update the product in dal
                    _dal.Product.Update(dalProduct);

                    // copy the orders and return the bl's one
                    return copyOrderFromDal(dalOrder, orderID);

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
