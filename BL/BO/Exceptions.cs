using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class UnvalidID : Exception
    {
        public UnvalidID() : base("The ID you entered isn't valid") { }
    }
    public class UnvalidName : Exception
    {
        public UnvalidName() : base("The name you entered isn't valid") { }
    }
    public class UnvalidAddress : Exception
    {
        public UnvalidAddress() : base("The address you entered isn't valid") { }
    }
    public class UnvalidEmail: Exception
    {
        public UnvalidEmail() : base("The email you entered isn't valid") { }
    }
    public class UnvalPrice : Exception
    {
        public UnvalPrice() : base("The price you entered isn't valid") { }
    }
    public class UnvalAmount : Exception
    {
        public UnvalAmount() : base("The amount you entered isn't valid") { }
    }
    public class DoesntExist : Exception
    {
        public DoesntExist() : base("The requested object doesn't exist") { }
        public DoesntExist(Exception ex) : base("The requested object doesn't exist", ex) { }
    }
    public class IdAlreadyExist : Exception
    {
        public IdAlreadyExist() : base("This ID already exist") { }
        public IdAlreadyExist(Exception ex) : base("This ID already exist",ex) { }
    }
    public class ProductExistInOrder : Exception
    {
        public ProductExistInOrder() : base("The product exist in an order, hence can not be deleted") { }
    }

    public class ProductNotInStock : Exception
    {
        public ProductNotInStock() : base("The product isn't in stock") { }
    }

    public class AlreadyShipped : Exception
    {
        public AlreadyShipped() : base("The order has already been shipped") { }
    }
}
