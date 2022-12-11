using BlApi;

namespace Bllmplementation
{
    // access to the BL main entities
    sealed public class Bl : IBl
    {
        public IProduct Product => new BlProduct();

        public IOrder Order => new BlOrder();

        public ICart Cart => new BlCart();
    }
}
