namespace DO
{
    [Serializable]
    public class NotFound : Exception
    {
        public NotFound() : base("The requested object wasn't found") { }
    }
    [Serializable]
    public class AlreadyExist : Exception
    {
        public AlreadyExist() : base("The object already exists") { }
    }
    [Serializable]
    public class Nullvalue : Exception
    {
        public Nullvalue() : base("NULL value") { }
    }
    [Serializable]
    public class DalConfigException : Exception
    {
        public DalConfigException(string msg) : base(msg) { }
        public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
    }
    [Serializable]
    public class FileLoadingError : Exception
    {
        public FileLoadingError() : base("The file can't be load") { }
    }
    [Serializable]
    public class FileSavingError : Exception
    {
        public FileSavingError() : base("The file can't be save") { }
    }
    [Serializable]
    public class XmlFormatError : Exception
    {
        public XmlFormatError(string field) : base(field + " format isn't valid") { }
    }
}
