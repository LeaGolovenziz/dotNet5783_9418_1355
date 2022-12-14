using BlApi;

namespace Bllmplementation
{
    // access to the BL main entities
    sealed internal class Bl : IBl
    {
        public IProduct Product { get; }

        public IOrder Order { get; }

        public ICart Cart { get; }

        public Bl()
        {
            Product = new BlProduct();
            Order = new BlOrder();
            Cart = new BlCart();
        }

    }
}
