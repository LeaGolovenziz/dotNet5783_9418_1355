using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using Dal;
using DalApi;

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
            throw new NotImplementedException();
        }

        void ICart.PlaceOrder(Cart cart, string name, string email, string address)
        {
            throw new NotImplementedException();
        }

        Cart ICart.UpdateProductAmountInCart(Cart cart, int productID, int amount)
        {
            throw new NotImplementedException();
        }
    }
}
