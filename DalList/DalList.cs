using DalApi;

namespace Dal
{
    internal sealed class DalList : IDal
    {
        public static IDal Instance { get; } = new DalList();
        public IOrder Order { get; }

        public IProduct Product { get; }

        public IOrderItem OrderItem { get; }

        public IUser User { get; }

        private DalList()
        {
            Order = new DalOrder();
            Product = new DalProduct();
            OrderItem = new DalOrderItem();
            User = new DalUser(); 
        }

    }
}
