using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    internal class User : IUser
    {
        string rootName = "users";
        string userPath = @"User.xml";

        /// <summary>
        /// Adds a new user to the file of the users
        /// </summary>
        /// <param name="user"></param>
        /// <returns>int</returns>
        /// <exception cref="FileSavingError"></exception>
        public int Add(DO.User user)
        {
            List<DO.User?> users = XmlTools.LoadListFromXMLSerializer<DO.User?>(userPath, rootName);

            try
            {
                Get(user.ID);
                throw new AlreadyExist();
            }
            catch (NotFound)
            {
                users.Add(user);

                XmlTools.SaveListToXMLSerializer(users, userPath);
            }
            return user.ID;
        }

        /// <summary>
        /// Deletes an order from the file of orders
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete(int id)
        {
            List<DO.User?> users = XmlTools.LoadListFromXMLSerializer<DO.User?>(userPath, rootName);

            users.Remove(Get(id));

            XmlTools.SaveListToXMLSerializer(users, userPath);
        }

        /// <summary>
        /// Update a user in the file of users
        /// </summary>
        /// <param name="t"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Update(DO.User user)
        {
            List<DO.User?> users = XmlTools.LoadListFromXMLSerializer<DO.User?>(userPath, rootName);

            users.Remove(Get(user.ID));
            users.Add(user);

            XmlTools.SaveListToXMLSerializer(users, userPath);
        }

        /// <summary>
        /// Returns list of all the users, if gets a condition - by it
        /// </summary>
        /// <param name="func"></param>
        /// <returns>IEnumerable<DO.Order?></returns>
        public IEnumerable<DO.User?> Get(Func<DO.User?, bool>? func = null)
        {
            List<DO.User?> users = XmlTools.LoadListFromXMLSerializer<DO.User?>(userPath, rootName);

            return func == null ? users : users.Where(func);
        }

        /// <summary>
        /// gets an ID and returns the user with this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DO.Order</returns>
        /// <exception cref="nullvalue"></exception>
        public DO.User Get(int id)
        {
            return GetIf(user => (user ?? throw new Nullvalue()).ID == id);
        }

        /// <summary>
        /// Gets a condition and returns a user with this condition
        /// </summary>
        /// <param name="func"></param>
        /// <returns>DO.User</returns>
        /// <exception cref="NotFound"></exception>
        public DO.User GetIf(Func<DO.User?, bool>? func)
        {
            List<DO.User?> users = XmlTools.LoadListFromXMLSerializer<DO.User?>(userPath, rootName);

            return users.FirstOrDefault(func!) ?? throw new NotFound();
        }

        public void ResetPassword(int ID, string password)
        {
            DO.User user = Get(ID);
            user.Password = password;
            Update(user);
        }
    }
}
