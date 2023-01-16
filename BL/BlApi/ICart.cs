using BO;
using DO;

namespace BlApi
{
    /// <summary>
    /// interface for cart methods
    /// </summary>
    public interface ICart
    {
        /// <summary>
        /// gets a cart and produc's id and adds the product to the cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="productID"></param>
        /// <returns>Cart</returns>
        public Cart AddProductToCart(Cart cart, int productID);
        /// <summary>
        ///  gets a cart, produc's id and an amount and update the amount in the cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="productID"></param>
        /// <param name="amount"></param>
        /// <returns>Cart</returns>
        public Cart UpdateProductAmountInCart(Cart cart, int productID, int amount);
        /// <summary>
        /// gets a cart and creates a new order, returns the new orders id
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="address"></param>
        public int PlaceOrder(Cart cart,BO.User user);
        /// <summary>
        /// gets a cart and products id and returns the amount un cart of the product
        /// </summary>
        /// <param name="productID"></param>
        /// <returns>int</returns>
        public int AmountOf(Cart cart, int productID);
    }
}
