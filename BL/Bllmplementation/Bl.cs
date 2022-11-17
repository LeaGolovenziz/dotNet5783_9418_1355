using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bllmplementation
{
    sealed public class Bl : IBl
    {
        public IProduct Product => new BlProduct();
       
        public IOrder Order => new BlOrder();

        public ICart Cart => new BlCart();  
    }
}
