﻿using BlApi;
using BO;
using Dal;
using DalApi;
using System.Net.Mail;

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
            // checks if product exists and in stock
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

            // if product's already in the order
            if (cart.OrderItems.Exists(x => x.ID == productID))
            {
                // find the index of the product 
                int index = cart.OrderItems.FindIndex(x => x.ID == productID);
                // adding 1 to the amount of the product
                cart.OrderItems.ElementAt(index).ProductAmount++;
                // updating the total price
                cart.OrderItems.ElementAt(index).TotalPrice += cart.OrderItems.ElementAt(index).Price;
                // updating the cart price
                cart.price += cart.OrderItems.ElementAt(index).Price;
            }
            else
            {
                // getting the product's details
                DO.Product product = _dal.Product.Get(productID);
                // creating a new order item for the cart and updating it's details
                BO.OrderItem orderItem = new BO.OrderItem();
                orderItem.ID = productID;
                orderItem.Price = product.Price;
                orderItem.ProductAmount = 1;
                orderItem.TotalPrice = product.Price;
                // adding the order item to the cart
                cart.OrderItems.Add(orderItem);
                //updating the cart total price
                cart.price += product.Price;
            }

            return cart;

        }

        void ICart.PlaceOrder(Cart cart)
        {
            // checking validity of customers details:

            if (cart.CustomerName!.Equals(""))
                throw new UnvalidName();
            // checking validity of email address
            try
            {
                MailAddress emailAddressAttribute = new MailAddress(cart.CustomerEmail!);
            }
            catch
            {
                throw new UnvalidEmail();
            }
            if (cart.CustomerAddress!.Equals(""))
                throw new UnvalidAddress();

            //checking validity of order items
            try
            {
                foreach (BO.OrderItem item in cart.OrderItems)
                {
                    DO.Product product = _dal.Product.Get(item.ID);
                    if (item.ProductAmount <= 0)
                        throw new UnvalidAmount();
                    if (_dal.Product.Get(product.ID).InStock < item.ProductAmount)
                        throw new ProductNotInStock();
                }
            }
            catch (NotFound e)
            {
                throw new DoesntExist();
            }

            // creating a new BO order
            BO.Order order = new BO.Order();
            order.CustomerAdress = cart.CustomerAddress;
            order.CustomerEmail = cart.CustomerEmail;
            order.CustomerName = cart.CustomerName;
            order.OrderStatus = BO.Enums.OrderStatus.Confirmed;
            order.OrderDate = DateTime.Now;
            order.Price = 0;
            order.OrderItems = new List<BO.OrderItem>();

            // creating a new DO order
            DO.Order tOrder = new DO.Order();
            tOrder.OrderDate = DateTime.Now;
            tOrder.CustomerAdress = cart.CustomerAddress;
            tOrder.CustomerEmail = cart.CustomerEmail;
            tOrder.CustomerName = cart.CustomerName;

            order.ID = _dal.Order.Add(tOrder);

            // adding order items from the cart to the order
            foreach (BO.OrderItem item in cart.OrderItems)
            {
                //creating a DO order item
                DO.OrderItem tOrderItem = new DO.OrderItem();
                tOrderItem.OrderID = order.ID;
                tOrderItem.ProductPrice = item.Price;
                tOrderItem.ProductID = item.ID;
                tOrderItem.ProductAmount = item.ProductAmount;
                _dal.OrderItem.Add(tOrderItem);

                // ading the BO order item to the order
                order.OrderItems.Add(item);

                // updating the amount of the product in dal
                try
                {
                    DO.Product product = _dal.Product.Get(item.ID);
                    product.InStock -= item.ProductAmount;
                    _dal.Product.Update(product);
                }
                catch (NotFound e)
                {
                    throw new DoesntExist(e);
                }
            }

        }

        Cart ICart.UpdateProductAmountInCart(Cart cart, int productID, int amount)
        {
            // if product doesn't exists in the cart throw exception
            if (!cart.OrderItems.Exists(x => x.ID == productID))
            {
                throw new DoesntExist();
            }

            // if the wanted amount is negative
            if (amount < 0)
            {
                throw new UnvalidAmount();
            }

            // find the index of the order item in the cart
            int index = cart.OrderItems.FindIndex(x => x.ID == productID);

            // if the wanted amount is bigger than the current amount 
            if (amount > cart.OrderItems.ElementAt(index).ProductAmount)
            {
                // checking if there is enough in stock
                if (_dal.Product.Get(productID).InStock < amount)
                {
                    throw new ProductNotInStock();
                }
                // updating the amount and the total price of the product
                cart.OrderItems.ElementAt(index).ProductAmount = amount;
                double totalPriceAdded = (double)cart.OrderItems.ElementAt(index).Price! * amount - (double)cart.OrderItems.ElementAt(index).TotalPrice!;
                cart.OrderItems.ElementAt(index).TotalPrice += totalPriceAdded;
                // updating the total price of the cart
                cart.price += totalPriceAdded;
            }
            // if the wanted amount equals 0
            else if (amount == 0)
            {
                double totalPriceSub = (double)cart.OrderItems.ElementAt(index).TotalPrice!;
                cart.OrderItems.RemoveAt(index);
                cart.price -= totalPriceSub;

            }
            // if the wanted amount is smaller than the current amount
            else if (amount < cart.OrderItems.ElementAt(index).ProductAmount)
            {
                int oldAmount = (int)cart.OrderItems.ElementAt(index).ProductAmount!;
                cart.OrderItems.ElementAt(index).ProductAmount = amount;
                double totalPriceSub = (double)cart.OrderItems.ElementAt(index).Price! * (oldAmount - amount);
                cart.OrderItems.ElementAt(index).TotalPrice -= totalPriceSub;
                cart.price -= totalPriceSub;
            }

            return cart;
        }
    }
}
