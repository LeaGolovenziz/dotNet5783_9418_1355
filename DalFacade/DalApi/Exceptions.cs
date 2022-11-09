using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public class NotFound
    {
        public static void Messege()
        {
            throw new Exception("The requested object wasn't found");
        }
    }

    public class AlreadyExist
    {
        public static string Messege()
        {
            throw new Exception("The object already exists");
        }
    }
}
