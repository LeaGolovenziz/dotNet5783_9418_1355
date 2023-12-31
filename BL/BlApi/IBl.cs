﻿namespace BlApi
{
    /// <summary>
    /// main interface of BL
    /// </summary>
    public interface IBl
    {
        /// <summary>
        /// unique object of product's interface
        /// </summary>
        public IProduct Product { get; }
        /// <summary>
        /// unique object of order's interface
        /// </summary>
        public IOrder Order { get; }
        /// <summary>
        /// unique object of cart's interface
        /// </summary>
        public ICart Cart { get; }
        /// <summary>
        /// unique object of user's interface
        /// </summary>
        public IUser User { get; }

    }
}
