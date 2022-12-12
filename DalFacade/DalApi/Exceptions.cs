namespace DalApi
{
    public class NotFound : Exception
    {
        public NotFound() : base("The requested object wasn't found") { }
    }

    public class AlreadyExist : Exception
    {
        public AlreadyExist() : base("The object already exists") { }
    }
    public class nullvalue : Exception
    {
        public nullvalue() : base("NULL value") { }
    }
}
