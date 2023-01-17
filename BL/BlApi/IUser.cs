using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IUser
    {
        /// <summary>
        /// Gets an user and adds it to the list of users
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user);

        /// <summary>
        /// Return a list of the users
        /// </summary>
        /// <returns>IEnumerable<User?></returns>
        public IEnumerable<User?> Get();

        /// <summary>
        /// Gets user's ID and returns user with this ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>User</returns>
        public User Get(int userID);

        /// <summary>
        /// Gets user's ID and returns the orders of this user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>IEnumerable<OrderForList></returns>
        public IEnumerable<OrderForList> GetUsersOrders(int userID);

        /// <summary>
        /// Gets user's ID and password and resets his password
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="password"></param>
        public void ResetPassword(int ID, string password);
    }
}
