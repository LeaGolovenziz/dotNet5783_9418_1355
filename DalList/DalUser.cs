using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class DalUser : IUser
    {
        /// <summary>
        /// adds the user to the users list and return it's id if doesn't already exist
        /// </summary>
        /// <param name="user"></param>
        /// <returns>int</returns>
        /// <exception cref="Exception"></exception>
        public int Add(User user)
        {
            if (DataSource.lstUsers.Exists(x => (x ?? throw new Nullvalue()).ID == user.ID))
                throw new AlreadyExist();
            DataSource.lstUsers.Add(user);
            return user.ID;
        }
        /// <summary>
        /// gets an ID and deletes the user with this ID
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            DataSource.lstUsers.Remove(Get(id));
        }
        /// <summary>
        /// return the list of users
        /// </summary>
        /// <returns>List<Order></returns>
        public IEnumerable<User?> Get(Func<User?, bool>? func = null)
        {
            if (func != null)
                return (from item in DataSource.lstUsers
                        where func(item)
                        select item).ToList();
            return DataSource.lstUsers;
        }

        /// <summary>
        /// gets an id and return the user with this id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product</returns>
        /// <exception cref="Exception"></exception>
        public User Get(int id)
        {
            return GetIf(user => (user ?? throw new Nullvalue()).ID == id);
        }
        /// <summary>
        ///  returns user who meets the condition
        /// </summary>
        /// <param name="func"></param>
        /// <returns>User</returns>
        /// <exception cref="NotFound"></exception>
        public User GetIf(Func<User?, bool>? func)
        {
            if (DataSource.lstUsers.Exists(x => func!(x)))
                return (User)DataSource.lstUsers.Find(x => func!(x))!;
            throw new NotFound();
        }

        public void ResetPassword(int ID, string password)
        {
            User user = Get(ID);
            user.Password = password;
            Update(user);
        }

        /// <summary>
        /// gets a user and updetes it
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user)
        {
            int index = DataSource.lstUsers.FindIndex(x => (x ?? throw new Nullvalue()).ID == user.ID);
            if (index == -1)
                throw new NotFound();
            DataSource.lstUsers[index] = user;
        }
    }
}
