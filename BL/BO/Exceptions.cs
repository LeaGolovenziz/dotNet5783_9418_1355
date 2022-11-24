using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class blExceptions: Exception
    {
        public blExceptions(string messege) : base(messege) { }
        public blExceptions(string messege,Exception ex) : base(messege,ex) { }
    }
    public class UnvalidID : blExceptions
    {
        public UnvalidID() : base("The ID you entered isn't valid") { }
    }
    public class UnvalidName : blExceptions
    {
        public UnvalidName() : base("The name you entered isn't valid") { }
    }
    public class UnvalidAddress : blExceptions
    {
        public UnvalidAddress() : base("The address you entered isn't valid") { }
    }
    public class UnvalidEmail: blExceptions
    {
        public UnvalidEmail() : base("The email you entered isn't valid") { }
    }
    public class UnvalidPrice : blExceptions
    {
        public UnvalidPrice() : base("The price you entered isn't valid") { }
    }
    public class UnvalidAmount : blExceptions
    {
        public UnvalidAmount() : base("The amount you entered isn't valid") { }
    }
    public class DoesntExist : blExceptions
    {
        public DoesntExist() : base("The requested object doesn't exist") { }
        public DoesntExist(Exception ex) : base("The requested object doesn't exist", ex) { }
    }
    public class IdAlreadyExist : blExceptions
    {
        public IdAlreadyExist() : base("This ID already exist") { }
        public IdAlreadyExist(Exception ex) : base("This ID already exist",ex) { }
    }
    public class ProductExistInOrder : blExceptions
    {
        public ProductExistInOrder() : base("The product exist in an order, hence can not be deleted") { }
    }

    public class ProductNotInStock : blExceptions
    {
        public ProductNotInStock() : base("The product isn't in stock") { }
    }

    public class AlreadyShipped : blExceptions
    {
        public AlreadyShipped() : base("The order has already been shipped") { }
    }
}
