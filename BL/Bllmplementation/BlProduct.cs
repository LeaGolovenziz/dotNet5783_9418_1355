using BO;
using Dal;
using DalApi;
using IProduct = BlApi.IProduct;

namespace Bllmplementation
{
    internal class BlProduct : IProduct
    {
        /// <summary>
        /// access to the dal entities
        /// </summary>
        private IDal _dal = new DalList();

        void IProduct.AddProduct(Product product)
        {
            // checks validity of product's types
            if (product.ID <= 0)
                throw new UnvalidID();
            if (product.Name!.Equals(""))
                throw new UnvalidName();
            if (product.Price <= 0)
                throw new UnvalidPrice();
            if (product.InStock < 0)
                throw new UnvalidAmount();

            // creates new DO product and copy into it the BO product's details
            DO.Product temp = product.CopyPropToStruct(new DO.Product());

            // add the DO product to dal's products list
            try
            {
                _dal.Product.Add(temp);
            }
            catch (AlreadyExist ex)
            {
                throw new IdAlreadyExist(ex);
            }
        }

        void IProduct.DeleteProduct(int productID)
        {
            // gets the order items list from dal
            IEnumerable<DO.OrderItem?> orderItems = _dal.OrderItem.Get();
            // checks foreach order item if contains the current product
            foreach (DO.OrderItem? item in orderItems)
            {
                if (item?.ProductID == productID)
                    throw new ProductExistInOrder();
            }

            // if the product doesn't exists in any order deletes the product from dal
            try
            {
                _dal.Product.Delete(productID);

            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        void IProduct.UpdateProduct(Product product)
        {
            // check validity of product's details
            if (product.ID <= 0)
                throw new UnvalidID();
            if (product.Name!.Equals(""))
                throw new UnvalidName();
            if (product.Price <= 0)
                throw new UnvalidPrice();
            if (product.InStock < 0)
                throw new UnvalidAmount();

            // creates new DO product and copy into it the BO product's details
            DO.Product temp =product.CopyPropToStruct( new DO.Product());

            // update the DO product in dal's products list
            try
            {
                _dal.Product.Update(temp);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        Product IProduct.GetProductDetails(int productID)
        {
            // if product doesn't exists in dal get function will throw an exception
            try
            {
                // checks validity of product's ID
                if (productID > 0)
                {
                    // get the DO product from dal
                    DO.Product temp = _dal.Product.Get(productID);

                    // creates new BO product and copy into it the DO product's details
                    BO.Product product =  temp.CopyPropTo(new BO.Product());

                    return product;
                }
                else
                    throw new UnvalidID();
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        ProductItem IProduct.GetProductFromCatalog(int productID, Cart cart)
        {
            // if product doesn't exists in dal get function will throw an exception
            try
            {
                // checks validity of product's ID
                if (productID > 0)
                {
                    // get the DO product from dal
                    DO.Product temp = _dal.Product.Get(productID);

                    // creates new BO product item and copy into it the DO product's details
                    ProductItem product = temp.CopyPropTo(new ProductItem());

                    if (temp.InStock == 0)
                        product.InStock = false;
                    else
                        product.InStock = true;

                    // count the amount of the product in the cart
                    product.AmountInCart = cart.OrderItems.Where(x => x.ID == productID).Count();

                    return product;
                }
                else
                    throw new UnvalidID();
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        IEnumerable<ProductForList?> IProduct.GetProductsList(Func<DO.Product?, bool>? func = null)
        {
            // creates list of BO ProductForList
            List<ProductForList> products = new List<ProductForList>();

            // get the list of DO products from dal
            IEnumerable<DO.Product?> lstProducts = _dal.Product.Get(func);

            // foreach DO product in th elist creat BO ProductForList and adds it to the list
            foreach (DO.Product? product in lstProducts)
            {
                ProductForList tempProduct =product.CopyPropTo(new ProductForList());

                products.Add(tempProduct);
            }
            return products;
        }
    }
}
