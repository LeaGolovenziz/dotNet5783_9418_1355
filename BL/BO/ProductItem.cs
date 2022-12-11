using static BO.Enums;

namespace BO
{
    public class ProductItem
    {
        /// <summary>
        /// unique ID of productItem
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// unique name of product
        /// </summary>
        public string? ProductName { get; set; }
        /// <summary>
        /// unique price of product
        /// </summary>
        public double? ProductPrice { get; set; }
        /// <summary>
        /// unique category of product
        /// </summary>
        public Category? ProductCategory { get; set; }
        /// <summary>
        /// unique is in stock flag
        /// </summary>
        public bool? IsInStock { get; set; }
        /// <summary>
        /// unique amount of product in cart
        /// </summary>
        public int? AmountInCart { get; set; }

        private string isInStock()
        {
            return (bool)IsInStock ? "true" : "false";
        }

        /// <summary>
        /// returns a string of the ordered item's details
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => $@"
        Product ID - {ProductID}:
        Product's name: {ProductName}   
                  price: {ProductPrice}
                  category: {ProductCategory}
        Is in stock: {isInStock()}
        Amount of product in cart: {AmountInCart}
";
    }
}
