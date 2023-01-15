using DalApi;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    public IProduct Product { get; }
    public IOrder Order { get; }
    public IOrderItem OrderItem { get; }
    public IUser User { get; }

    private DalXml()
    {
        Product = new Dal.Product();
        Order = new Dal.Order();
        OrderItem = new Dal.OrderItem();
        User = new Dal.User();
    }
}
