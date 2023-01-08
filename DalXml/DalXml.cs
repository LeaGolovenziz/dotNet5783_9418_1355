using DalApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    sealed internal class DalXml:IDal
    {
        public static IDal Instance { get; } = new DalXml();
        public IProduct Product { get; } 
        public IOrder Order { get; }
        public IOrderItem OrderItem { get; }
        private DalXml()
        {
            Product = new Dal.Product();
            Order = new Dal.Order();
            OrderItem = new Dal.OrderItem();
        }
    }
}
