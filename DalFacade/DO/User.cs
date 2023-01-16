using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct User
    {
        /// <summary>
        /// Unique ID of User
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Unique name of User
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Unique password of User
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Unique is maneger of user
        /// </summary>
        public bool IsManeger { get; set; }

        /// <summary>
        /// returns a string of the order's details
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => this.ToStringProperty();
    }
}
