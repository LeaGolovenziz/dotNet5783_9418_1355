using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public class NotFound:Exception
    {
        public NotFound():base("The requested object wasn't found") {}
    }

    public class AlreadyExist : Exception
    {
        public AlreadyExist():base("The object already exists") { }
    }
}
