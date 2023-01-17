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
        public void AddUser(User user);

        public IEnumerable<User?> Get();

        public User Get(int userID);

        public IEnumerable<OrderForList> GetUsersOrders(int userID);

        public void ResetPassword(int ID, string password);

    }
}
