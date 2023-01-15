namespace BO
{
    [Serializable]
    public class blExceptions : Exception
    {
        public blExceptions(string messege) : base(messege) { }
        public blExceptions(string messege, Exception ex) : base(messege, ex) { }
    }
    [Serializable]
    public class UnvalidID : blExceptions
    {
        public UnvalidID() : base("The ID you entered isn't valid") { }
    }
    [Serializable]
    public class UnvalidName : blExceptions
    {
        public UnvalidName() : base("The name you entered isn't valid") { }
    }
    [Serializable]
    public class UnvalidAddress : blExceptions
    {
        public UnvalidAddress() : base("The address you entered isn't valid") { }
    }
    [Serializable]
    public class UnvalidEmail : blExceptions
    {
        public UnvalidEmail() : base("The email you entered isn't valid") { }
    }
    [Serializable]
    public class UnvalidPrice : blExceptions
    {
        public UnvalidPrice() : base("The price you entered isn't valid") { }
    }
    [Serializable]
    public class UnvalidAmount : blExceptions
    {
        public UnvalidAmount() : base("The amount you entered isn't valid") { }
    }
    [Serializable]
    public class DoesntExist : blExceptions
    {
        public DoesntExist() : base("The requested object doesn't exist") { }
        public DoesntExist(Exception ex) : base("The requested object doesn't exist", ex) { }
    }
    [Serializable]
    public class IdAlreadyExist : blExceptions
    {
        public IdAlreadyExist() : base("This ID already exist") { }
        public IdAlreadyExist(Exception ex) : base("This ID already exist", ex) { }
    }
    [Serializable]
    public class ProductExistInOrder : blExceptions
    {
        public ProductExistInOrder() : base("The product exist in an order, hence can not be deleted") { }
    }
    [Serializable]
    public class ProductNotInStock : blExceptions
    {
        public ProductNotInStock() : base("The product isn't in stock") { }
    }
    [Serializable]
    public class AlreadyShipped : blExceptions
    {
        public AlreadyShipped() : base("The order has already been shipped") { }
    }
    [Serializable]
    public class AlreadyDelivered : blExceptions
    {
        public AlreadyDelivered() : base("The order has already been delivered") { }
    }
    [Serializable]
    public class wasntShipped : blExceptions
    {
        public wasntShipped() : base("The order wasn't shipped") { }
    }
    [Serializable]
    public class Nullvalue : blExceptions
    {
        public Nullvalue() : base("NULL value") { }
    }
    [Serializable]
    public class FileLoadingError : blExceptions
    {
        public FileLoadingError(DO.FileLoadingError ex) : base("The file can't be load", ex) { }
    }
    [Serializable]
    public class FileSavingError : blExceptions
    {
        public FileSavingError(DO.FileSavingError ex) : base("The file can't be save", ex) { }
    }
    [Serializable]
    public class XmlFormatError : blExceptions
    {
        public XmlFormatError(DO.XmlFormatError ex) : base("wrong xml element format", ex) { }
    }
}
