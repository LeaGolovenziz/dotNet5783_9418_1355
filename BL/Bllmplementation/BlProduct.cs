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
            if (product.ID <= 0)
                throw new UnvalidID();
            if (product.Name.Equals(""))
                throw new UnvalidName();
            if (product.Price <= 0)
                throw new UnvalPrice();
            if (product.InStock < 0)
                throw new UnvalidAmount();

            DO.Product temp = new DO.Product();
            temp.ID = product.ID;
            temp.Name = product.Name;
            temp.Price = product.Price;
            temp.InStock = product.InStock;
            temp.Category = (DO.Enums.Category)product.Category;
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
            IEnumerable<DO.OrderItem> orderItems = _dal.OrderItem.Get();
            foreach(DO.OrderItem item in orderItems)
            {
                if (item.ProductID == productID)
                    throw new ProductExistInOrder();
            }
            try
            {
                _dal.OrderItem.Delete(productID);
            }
            catch(NotFound ex)
            {
                throw new DoesntExist(ex);
            }
        }

        void IProduct.UpdateProduct(Product product)
        {
            if (product.ID <= 0)
                throw new UnvalidID();
            if (product.Name.Equals(""))
                throw new UnvalidName();
            if (product.Price <= 0)
                throw new UnvalPrice();
            if (product.InStock < 0)
                throw new UnvalidAmount();

            DO.Product temp = new DO.Product();
            temp.ID = product.ID;
            temp.Name = product.Name;
            temp.Price = product.Price;
            temp.InStock = product.InStock;
            temp.Category = (DO.Enums.Category)product.Category;
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
            try
            {
                if (productID > 0)
                {
                    DO.Product temp = _dal.Product.Get(productID);
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
            try
            {
                if (productID > 0)
                {
                    DO.Product temp = _dal.Product.Get(productID);
                    ProductItem product = new ProductItem();
                    product.ProductID = temp.ID;
                    product.ProductName = temp.Name;
                    product.ProductPrice = temp.Price;
                    product.ProductCategory = (BO.Enums.Category)temp.Category;
                    if (temp.InStock==0)
                        product.IsInStock = false;
                    else
                        product.IsInStock = true;
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

        IEnumerable<ProductForList> IProduct.GetProductsList()
        {
            List<ProductForList> products=new List<ProductForList>();
            IEnumerable<DO.Product> lstProducts = _dal.Product.Get();
            foreach(DO.Product product in lstProducts)
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
