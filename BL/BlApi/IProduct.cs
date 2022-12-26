using BO;

namespace BlApi
{
    /// <summary>
    /// interface for product methods
    /// </summary>
    public interface IProduct
    {
        /// <summary>
        /// return a list of the ProductForList (for manager)
        /// </summary>
        /// <returns>IEnumerable<ProductForList></returns>
        IEnumerable<ProductForList?> GetProductsList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="productsForList"></param>
        /// <returns></returns>
        IEnumerable<ProductForList?> GetProductsListByCondition(Func<BO.ProductForList?, bool>? func, IEnumerable<ProductForList?> productsForList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public ProductForList GetProductForList(int productID);

        /// <summary>
        /// gets product's id and returns Product (for manager)
        /// </summary>
        /// <returns>Product</returns>
        public Product GetProductDetails(int productID);

        /// <summary>
        /// gets product's id and returns ProductItem (for customer)
        /// </summary>
        /// <returns>ProductItem</returns>
        public ProductItem GetProductFromCatalog(int productID, Cart cart);
        /// <summary>
        /// gets product and adds it to the list of products (for manager)
        /// </summary>
        /// <returns>Product</returns>
        public void AddProduct(Product product);
        /// <summary>
        /// gets product's id and deletes it from the list of products (for manager)
        /// </summary>
        /// <returns>Product</returns>
        public void DeleteProduct(int productID);
        /// <summary>
        /// gets product and adds it to the list of products (for manager)
        /// </summary>
        /// <returns>Product</returns>
        public void UpdateProduct(Product product);

    }
}
