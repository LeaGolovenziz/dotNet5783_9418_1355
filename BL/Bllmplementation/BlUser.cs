using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bllmplementation
{
    internal class BlUser : BlApi.IUser
    {
        /// <summary>
        /// access to the dal entities
        /// </summary>
        private IDal? dal = DalApi.Factory.Get();
        public void AddUser(BO.User user)
        {
            if (user.ID <= 0||user.ID.ToString().Length!=9)
                throw new UnvalidID();

            // creates new DO user and copy into it the BO user's details
            DO.User dalUser = user.CopyPropToStruct( new DO.User());

            // add the DO product to dal's products list
            try
            {
                dal.User.Add(dalUser);
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

        public BO.User Get(int userID)
        {
           return dal.User.Get(userID).CopyPropTo(new BO.User());
        }

        public IEnumerable<BO.User?> Get()
        {
            // foreach DO user in th elist creat BO user and adds it to the list
            IEnumerable<DO.User?> p = dal.User.Get();
            IEnumerable<BO.User?> pfl = p.Select(product => product.CopyPropTo(new BO.User()));
            return pfl;
        }

        public IEnumerable<OrderForList> GetUsersOrders(int userID)
        {
            if (dal.User.Get(userID).IsManeger == true)
                return BlApi.Factory.Get().Order.GetOrderList()!;
           else
                return BlApi.Factory.Get().Order.GetOrderList().Where(order => (order ?? throw new BO.Nullvalue()).CustomerID == userID)!;
        }

        public void ResetPassword(int ID, string password)
        {
            dal.User.ResetPassword(ID, password);
        }
    }
}
