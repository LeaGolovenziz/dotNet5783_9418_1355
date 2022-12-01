using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
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
            if (product.Name.Equals(""))
                throw new UnvalidName();
            if (product.Price <= 0)
                throw new UnvalidPrice();
            if (product.InStock < 0)
                throw new UnvalidAmount();

            // creates new DO product and copy into it the BO product's details
            DO.Product temp = new DO.Product();
            temp.ID = product.ID;
            temp.Name = product.Name;
            temp.Price = product.Price;
            temp.InStock = product.InStock;
            temp.Category = (DO.Enums.Category)product.Category;

            // add the DO product to dal's products list
            try
            {
                _dal.Product.Add(temp);
            }
            catch(AlreadyExist ex)
            {
                throw new IdAlreadyExist(ex);
            }
        }

        void IProduct.DeleteProduct(int productID)
        {
            // gets the order items list from dal
            IEnumerable<DO.OrderItem?> orderItems = _dal.OrderItem.Get();
            // checks foreach order item if contains the current product
            foreach(DO.OrderItem item in orderItems)
            {
                if (item.ProductID == productID)
                    throw new ProductExistInOrder();
            }

            // if the product doesn't exists in any order deletes the product from dal
            try
            {
                _dal.Product.Delete(productID);

            }
            catch(NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        void IProduct.UpdateProduct(Product product)
        {
            // check validity of product's details
            if (product.ID <= 0)
                throw new UnvalidID();
            if (product.Name.Equals(""))
                throw new UnvalidName();
            if (product.Price <= 0)
                throw new UnvalidPrice();
            if (product.InStock < 0)
                throw new UnvalidAmount();

            // creates new DO product and copy into it the BO product's details
            DO.Product temp = new DO.Product();
            temp.ID = product.ID;
            temp.Name = product.Name;
            temp.Price = product.Price;
            temp.InStock = product.InStock;
            temp.Category = (DO.Enums.Category)product.Category;

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
                    BO.Product product = new BO.Product();
                    product.ID = temp.ID;
                    product.Name = temp.Name;
                    product.Price = temp.Price;
                    product.Category = (BO.Enums.Category)temp.Category;
                    product.InStock = temp.InStock;

                    return product;
                }
                else
                    throw new UnvalidID();
            }
            catch(NotFound ex)
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
                    ProductItem product = new ProductItem();
                    product.ProductID = temp.ID;
                    product.ProductName = temp.Name;
                    product.ProductPrice = temp.Price;
                    product.ProductCategory = (BO.Enums.Category)temp.Category;

                    if (temp.InStock==0)
                        product.IsInStock = false;
                    else
                        product.IsInStock = true;

                    // count the amount of the product in the cart
                    product.AmountInCart = cart.OrderItems.Where(x=>x.ProductID == productID).Count();

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

        IEnumerable<ProductForList?> IProduct.GetProductsList()
        {
            // creates list of BO ProductForList
            List<ProductForList> products=new List<ProductForList>();

            // get the list of DO products from dal
            IEnumerable<DO.Product?> lstProducts = _dal.Product.Get();

            // foreach DO product in th elist creat BO ProductForList and adds it to the list
            foreach (DO.Product product in lstProducts)
            {
                ProductForList tempProduct = new ProductForList();
                tempProduct.ProductID = product.ID;
                tempProduct.Name = product.Name;
                tempProduct.Price = product.Price;
                tempProduct.Category = (BO.Enums.Category)product.Category;
                products.Add(tempProduct);
            }
            return products;
        }
    }
}
