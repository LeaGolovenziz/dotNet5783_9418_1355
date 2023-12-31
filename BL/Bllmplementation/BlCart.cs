﻿using BlApi;
using BO;
using DalApi;
using DO;
using System.Net.Mail;

namespace Bllmplementation
{
    internal class BlCart : ICart
    {
        /// <summary>
        /// access to the dal entities
        /// </summary>
        private IDal? dal = DalApi.Factory.Get();

        /// <summary>
        /// returns the ampunt of the product in the cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="productID"></param>
        /// <returns>int</returns>
        public int AmountOf(Cart cart, int productID)
        {
            if (cart.OrderItems == null)
                return 0;
            return (int)(cart.OrderItems.FirstOrDefault(x => x.ID == productID)?.ProductAmount ?? 0);
        }

        Cart ICart.AddProductToCart(Cart cart, int productID)
        {
            // checks if product exists and in stock
            try
            {
                if (dal.Product.Get(productID).InStock == 0)
                {
                    throw new ProductNotInStock();
                }
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
            if (cart.OrderItems == null)
            {
                cart.OrderItems = new List<BO.OrderItem>();
                cart.Price = 0;
            }

            // if product's already in the order
            if (cart.OrderItems.Exists(x => x.ID == productID))
            {
                // find the index of the product 
                int index = cart.OrderItems.FindIndex(x => x.ID == productID);
                // chek if there is enough in stock
                if (dal.Product.Get(productID).InStock - cart.OrderItems.ElementAt(index).ProductAmount <= 0)
                {
                    throw new ProductNotInStock();
                }
                // adding 1 to the amount of the product
                cart.OrderItems.ElementAt(index).ProductAmount++;
                // updating the total price
                cart.OrderItems.ElementAt(index).TotalPrice += cart.OrderItems.ElementAt(index).Price;
                // updating the cart price
                cart.Price += cart.OrderItems.ElementAt(index).Price;
            }
            else
            {
                // getting the product's details
                DO.Product product = dal.Product.Get(productID);
                // creating a new order item for the cart and updating it's details
                BO.OrderItem orderItem = new BO.OrderItem
                {
                    ID = productID,
                    Name = product.Name,
                    Price = product.Price,
                    ProductAmount = 1,
                    TotalPrice = product.Price
                };

                // adding the order item to the cart
                cart.OrderItems.Add(orderItem);
                //updating the cart total price
                cart.Price += product.Price;
            }
            return cart;
        }

        int ICart.PlaceOrder(Cart cart,BO.User user)
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
                    DO.Product product = dal.Product.Get(item.ID);
                    if (item.ProductAmount <= 0)
                        throw new UnvalidAmount();
                    if (dal.Product.Get(product.ID).InStock < item.ProductAmount)
                        throw new ProductNotInStock();
                }
            }
            catch (NotFound e)
            {
                throw new DoesntExist();
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

            // creating a new BO order
            BO.Order order = new BO.Order()
            {
                CustomerAdress = cart.CustomerAddress,
                CustomerEmail = cart.CustomerEmail,
                CustomerName = cart.CustomerName,
                CustomerID=user.ID,
                OrderStatus = BO.Enums.OrderStatus.Confirmed,
                OrderDate = DateTime.Now,
                Price = 0,
                OrderItems = new List<BO.OrderItem>(),
            };

            // creating a new DO order
            DO.Order tOrder = new DO.Order()
            {
                OrderDate = DateTime.Now,
                CustomerAdress = cart.CustomerAddress,
                CustomerEmail = cart.CustomerEmail,
                CustomerName = cart.CustomerName,
                CustomerID=user.ID
            };

            order.ID = dal.Order.Add(tOrder);

            // adding order items from the cart to the order
            foreach (BO.OrderItem item in cart.OrderItems)
            {
                //creating a DO order item
                DO.OrderItem tOrderItem = new DO.OrderItem()
                {
                    OrderID = order.ID,
                    Price = item.Price,
                    ID = item.ID,
                    ProductAmount = item.ProductAmount,
                };
                dal.OrderItem.Add(tOrderItem);

                // ading the BO order item to the order
                order.OrderItems.Add(item);

                // updating the amount of the product in dal
                try
                {
                    DO.Product product = dal.Product.Get(item.ID);
                    product.InStock -= item.ProductAmount;
                    dal.Product.Update(product);
                }
                catch (NotFound e)
                {
                    throw new DoesntExist(e);
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
            return order.ID;

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
                if (dal.Product.Get(productID).InStock < amount)
                {
                    throw new ProductNotInStock();
                }
                // updating the amount and the total price of the product
                cart.OrderItems.ElementAt(index).ProductAmount = amount;
                double totalPriceAdded = (double)cart.OrderItems.ElementAt(index).Price! * amount - (double)cart.OrderItems.ElementAt(index).TotalPrice!;
                cart.OrderItems.ElementAt(index).TotalPrice += totalPriceAdded;
                // updating the total price of the cart
                cart.Price += totalPriceAdded;
            }
            // if the wanted amount equals 0
            else if (amount == 0)
            {
                double totalPriceSub = (double)cart.OrderItems.ElementAt(index).TotalPrice!;
                cart.OrderItems.RemoveAt(index);
                cart.Price -= totalPriceSub;

            }
            // if the wanted amount is smaller than the current amount
            else if (amount < cart.OrderItems.ElementAt(index).ProductAmount)
            {
                int oldAmount = (int)cart.OrderItems.ElementAt(index).ProductAmount!;
                cart.OrderItems.ElementAt(index).ProductAmount = amount;
                double totalPriceSub = (double)cart.OrderItems.ElementAt(index).Price! * (oldAmount - amount);
                cart.OrderItems.ElementAt(index).TotalPrice -= totalPriceSub;
                cart.Price -= totalPriceSub;
            }

            return cart;
        }
    }
}
