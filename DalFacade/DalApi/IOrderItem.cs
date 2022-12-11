using DO;

namespace DalApi
{
    public interface IOrderItem : ICrud<OrderItem>
    {
        public OrderItem Get(int productID, int orderID);
        public IEnumerable<OrderItem?> GeOrderItems(int orderID);
    }
}
