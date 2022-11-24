using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using Dal;
using DalApi;
using System.ComponentModel.DataAnnotations;
using DO;

namespace Bllmplementation
{
    internal class BlCart : ICart
    {
        /// <summary>
        /// access to the dal entities
        /// </summary>
        private IDal _dal = new DalList();

        Cart ICart.AddProductToCart(Cart cart, int productID)
        {
            try
            {
                if (_dal.Product.Get(productID).InStock == 0)
                {
                    throw new ProductNotInStock();
                }
            }
            catch (NotFound ex)
            {
                throw new DoesntExist(ex);
            }

            if (cart.OrderItems.Exists(x => x.ProductID == productID))
            {
                int index = cart.OrderItems.FindIndex(x => x.ProductID == productID);
                cart.OrderItems.ElementAt(index).ProductAmount++;
                cart.OrderItems.ElementAt(index).TotalPrice += cart.OrderItems.ElementAt(index).ProductPrice;
                cart.price += cart.OrderItems.ElementAt(index).ProductPrice;
            }
            else
            {
                DO.Product product = _dal.Product.Get(productID);
                BO.OrderItem orderItem = new BO.OrderItem();
                orderItem.ProductID = productID;
                orderItem.ProductPrice = product.Price;
                orderItem.ProductAmount = 1;
                orderItem.TotalPrice = product.Price;
                cart.OrderItems.Add(orderItem);
                cart.price += product.Price;
            }

            return cart;

        }

        void ICart.PlaceOrder(Cart cart, string name, string email, string address)
        {
            if (name.Equals(""))
                throw new UnvalidName();
            // for cheking email validation
            EmailAddressAttribute emailAddressAttribute = new EmailAddressAttribute();
            if (emailAddressAttribute.IsValid(email))
                throw new UnvalidEmail();
            if (address.Equals(""))
                throw new UnvalidAddress();
            try
            {
                foreach (BO.OrderItem item in cart.OrderItems)
                {
                    DO.Product product = _dal.Product.Get(item.ProductID);
                    if (item.ProductAmount <= 0)
                        throw new UnvalidAmount();
                    if(_dal.Product.Get(product.ID).InStock < item.ProductAmount)
                        throw new ProductNotInStock();
                }
            }
            catch (NotFound e)
            {
                throw new DoesntExist();
            }

            BO.Order order= new BO.Order();
            order.CustumerAdress = address;
            order.CustumerEmail = email;
            order.CustumerName=name;
            order.OrderStatus = BO.Enums.OrderStatus.Confirmed;
            order.OrderDate = DateTime.Now;
            order.Price = 0;
            order.OrderItems = new List<BO.OrderItem>();

            DO.Order tOrder = new DO.Order();
            tOrder.OrderDate = DateTime.Now;
            tOrder.CustomerAdress = address;
            tOrder.CustomerEmail = email;
            tOrder.CustomerName = name;

            order.ID = _dal.Order.Add(tOrder);

            foreach (BO.OrderItem item in order.OrderItems)
            {
                DO.OrderItem tOrderItem = new DO.OrderItem();
                tOrderItem.OrderID=order.ID;
                tOrderItem.Price = item.ProductPrice;
                tOrderItem.ProductID = item.ProductID;
                tOrderItem.Amount = item.ProductAmount;
                _dal.OrderItem.Add(tOrderItem);

                order.OrderItems.Add(item);

                try
                {
                    DO.Product product = _dal.Product.Get(item.ProductID);
                    product.InStock -= item.ProductAmount;
                    _dal.Product.Update(product);
                }
                catch (NotFound e)
                {
                    throw new DoesntExist();
                }
            }

        }

        Cart ICart.UpdateProductAmountInCart(Cart cart, int productID, int amount)
        {
            if (!cart.OrderItems.Exists(x => x.ProductID == productID))
            {
                throw new DoesntExist();
            }
            int index = cart.OrderItems.FindIndex(x => x.ProductID == productID);
            if (amount > cart.OrderItems.ElementAt(index).ProductAmount)
            {
                if (_dal.Product.Get(productID).InStock < (cart.OrderItems.ElementAt(index).ProductAmount - amount))
                {
                    throw new ProductNotInStock();
                }
                cart.OrderItems.ElementAt(index).ProductAmount += amount;
                double totalPriceAdded = (double)cart.OrderItems.ElementAt(index).ProductPrice * amount;
                cart.OrderItems.ElementAt(index).TotalPrice += totalPriceAdded;
                cart.price += totalPriceAdded;
            }
            else if (amount < cart.OrderItems.ElementAt(index).ProductAmount)
            {
                cart.OrderItems.ElementAt(index).ProductAmount -= amount;
                double totalPriceSub = (double)cart.OrderItems.ElementAt(index).ProductPrice * amount;
                if (cart.OrderItems.ElementAt(index).ProductAmount == 0)
                    cart.OrderItems.RemoveAt(index);
                else
                    cart.OrderItems.ElementAt(index).TotalPrice -= totalPriceSub;
                cart.price -= totalPriceSub;
            }
            return cart;
        }
    }
}
