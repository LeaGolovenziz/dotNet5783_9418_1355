namespace DO
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
    [Serializable]
    public class DalConfigException : Exception
    {
        public DalConfigException(string msg) : base(msg) { }
        public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
    }

}
