using BO;
using DalApi;
using DO;
using IProduct = BlApi.IProduct;
using Nullvalue = BO.Nullvalue;

namespace Bllmplementation
{
    internal class BlProduct : IProduct
    {
        /// <summary>
        /// access to the dal entities
        /// </summary>
        private IDal? dal = DalApi.Factory.Get();

        void IProduct.AddProduct(BO.Product product)
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
                dal.Product.Add(temp);
            }
            catch (AlreadyExist ex)
            {
                throw new IdAlreadyExist(ex);
            }
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
            }
        }

        void IProduct.DeleteProduct(int productID)
        {
            // gets the order items list from dal
            IEnumerable<DO.OrderItem?> orderItems = dal.OrderItem.Get();
            // checks foreach order item if contains the current product
            if (orderItems.FirstOrDefault(OrderItem => (OrderItem ?? throw new Nullvalue()).ID == productID) == null)
                throw new ProductExistInOrder();

            // if the product doesn't exists in any order deletes the product from dal
            try
            {
                dal.Product.Delete(productID);

            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
            }
        }

        void IProduct.UpdateProduct(BO.Product product)
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
            DO.Product temp = product.CopyPropToStruct(new DO.Product());

            // update the DO product in dal's products list
            try
            {
                dal.Product.Update(temp);
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
            }
        }

        BO.Product IProduct.GetProductDetails(int productID)
        {
            // if product doesn't exists in dal get function will throw an exception
            try
            {
                // checks validity of product's ID
                if (productID > 0)
                {
                    // get the DO product from dal
                    DO.Product temp = dal.Product.Get(productID);

                    // creates new BO product and copy into it the DO product's details
                    BO.Product product = temp.CopyPropTo(new BO.Product());

                    return product;
                }
                else
                    throw new UnvalidID();
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
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
                    DO.Product temp = dal.Product.Get(productID);

                    // creates new BO product item and copy into it the DO product's details
                    ProductItem product = temp.CopyPropTo(new ProductItem());

                    if (temp.InStock == 0)
                        product.InStock = false;
                    else
                        product.InStock = true;

                    // count the amount of the product in the cart
                    if (cart.OrderItems != null && cart.OrderItems.Exists(x => x.ID == productID))
                        product.AmountInCart = cart.OrderItems.Find(x => x.ID == productID)!.ProductAmount;
                    else
                        product.AmountInCart = 0;

                    return product;
                }
                else
                    throw new UnvalidID();
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }
            catch (DO.FileSavingError ex)
            {
                throw new BO.FileSavingError(ex);
            }
            catch (DO.FileLoadingError ex)
            {
                throw new BO.FileLoadingError(ex);
            }
            catch (DO.XmlFormatError ex)
            {
                throw new BO.XmlFormatError(ex);
            }
        }

        IEnumerable<ProductForList?> IProduct.GetProductsList()
        {
            // foreach DO product in th elist creat BO ProductForList and adds it to the list
            IEnumerable<DO.Product?> p = dal.Product.Get();
            IEnumerable<ProductForList?> pfl = p.Select(product => product.CopyPropTo(new ProductForList()));
            return pfl;
        }

        public ProductForList GetProductForList(int productID)
                =>
                 productID > 0 ? dal.Product.Get(productID).CopyPropTo(new ProductForList()) :
                    throw new UnvalidID();
    }
}
