using BL;
using BO;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using static BO.Enums;
using System.Xml.Linq;
using BlApi;
using Bllmplementation;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Collections.Specialized;

namespace BlTest
{
    internal class Program
    {
        private static IBl _iBl = new Bl();

        static void Main()
        {
            // gets customer details for cart
            void getCartDetails(ref Cart cart)
            {
                Console.WriteLine("Enter customer's name:");
                string name = Console.ReadLine();
                cart.CustomerName = name;

                Console.WriteLine("Enter customer's email:");
                string email = Console.ReadLine();
                cart.CustomerEmail = email;

                Console.WriteLine("Enter customer's address:");
                string address = Console.ReadLine();
                cart.CustomerAddress = address;
            }

            // gets product that have an ID, inputs its other details and returns it
            void GetProductDetails(ref Product product)
            {
                Console.WriteLine("Enter product's ID:");
                int id = int.Parse(Console.ReadLine());
                product.ID = id;

                Console.WriteLine("Enter product's name:");
                string name = Console.ReadLine();
                product.Name = name;

                Console.WriteLine("Enter product's price:");
                double price = double.Parse(Console.ReadLine());
                product.Price = price;

                Console.WriteLine("Enter product's category:");
                string category1 = Console.ReadLine();
                Enums.Category category = (Enums.Category)Enum.Parse(typeof(Enums.Category), category1);
                product.Category = category;

                Console.WriteLine("Enter product's amount in stock:");
                string tempInStock = Console.ReadLine();
                int inStock;
                int.TryParse(tempInStock, out inStock);
                product.InStock = inStock;
            }

            // gets an exception and prints its messeges
            void catchErrors(blExceptions ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.GetType().Name);
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.GetType().Name);
                    Console.WriteLine(ex.InnerException.Message);
                }
                Console.WriteLine();
            }

            int firstChoice = 0, orderID, productID;
            Order order = new Order();
            Product product = new Product();
            Cart cart = new Cart();

            // initializing the cart's items list
            cart.OrderItems = new List<OrderItem>();

            // for geting int from the user using tryParse
            string tempProductsID;

            ProductItem productItem;

            do
            {
                Console.WriteLine("1. check order\n2. check product\n3. check cart \n0. to exit\n");
                firstChoice = int.Parse(Console.ReadLine());
                char secondChoise;
                string tempOrderID, tempSecondChoise, tempProductID, tempNewAmount;
                int newAmount;

                switch (firstChoice)
                {
                    // exit
                    case 0:
                        Console.WriteLine("bye");
                        break;

                    // order operations
                    case 1:
                        try
                        {
                            do
                            {
                                Console.WriteLine("g. get orders' list\no. get order's details\nd. update order's delivery\ns. update order's shipping\nt. track order\nu. update order's details\ne. return to main menue\n");
                                tempSecondChoise = Console.ReadLine();
                                char.TryParse(tempSecondChoise, out secondChoise);

                                switch (secondChoise)
                                {
                                    // get order's list
                                    case 'g':
                                        IEnumerable<OrderForList> lst = _iBl.Order.GetOrderList();
                                        foreach (OrderForList orderInList in lst)
                                        {
                                            Console.WriteLine(orderInList);
                                        }
                                        break;

                                    // get order details
                                    case 'o':
                                        // input order's ID
                                        Console.WriteLine("Enter order's id");
                                        tempOrderID = Console.ReadLine();
                                        int.TryParse(tempOrderID, out orderID);

                                        order = _iBl.Order.GetOrderDetails(orderID);

                                        Console.WriteLine(order);
                                        break;

                                    // update order's delivery
                                    case 'd':
                                        Console.WriteLine("Enter order's id");
                                        tempOrderID = Console.ReadLine();
                                        int.TryParse(tempOrderID, out orderID);

                                        order = _iBl.Order.DeliverOrder(orderID);

                                        Console.WriteLine(order);

                                        break;

                                    // update order's shipping
                                    case 's':
                                        Console.WriteLine("Enter order's id");
                                        tempOrderID = Console.ReadLine();
                                        int.TryParse(tempOrderID, out orderID);

                                        order = _iBl.Order.ShipOrder(orderID);

                                        Console.WriteLine(order);

                                        break;

                                    // track order
                                    case 't':
                                        Console.WriteLine("Enter order's id");
                                        tempOrderID = Console.ReadLine();
                                        int.TryParse(tempOrderID, out orderID);

                                        OrderTracking orderT = _iBl.Order.TrackOrder(orderID);

                                        Console.WriteLine(orderT);

                                        break;

                                    // update order's details
                                    case 'u':
                                        // input details of the update
                                        Console.WriteLine("Enter order's id");
                                        tempOrderID = Console.ReadLine();
                                        int.TryParse(tempOrderID, out orderID);
                                        Console.WriteLine("Enter product's id");
                                        tempProductID = Console.ReadLine();
                                        int.TryParse(tempProductID, out productID);
                                        Console.WriteLine("Enter new amount");
                                        tempNewAmount = Console.ReadLine();
                                        int.TryParse(tempNewAmount, out newAmount);

                                        order = _iBl.Order.UpdateOrderDetails(orderID, productID, newAmount);

                                        Console.WriteLine(order);

                                        break;
                                }
                            }
                            while (!secondChoise.Equals('e'));
                        }
                        catch (blExceptions ex)
                        {
                            catchErrors(ex);
                        }

                        break;
                    // product operations
                    case 2:
                        try
                        {
                            do
                            {

                                Console.WriteLine("g. get products' list\np. get product's details\nc. get product from catalog\na. add product\nd. delete product\nu. update product's details\ne. return to main menue\n");
                                tempSecondChoise = Console.ReadLine();
                                char.TryParse(tempSecondChoise, out secondChoise);
                                switch (secondChoise)
                                {
                                    // get and print product's list
                                    case 'g':
                                        IEnumerable<ProductForList> products = _iBl.Product.GetProductsList();
                                        foreach (ProductForList productForList in products)
                                        {
                                            Console.WriteLine(productForList);
                                        }
                                        break;
                                    // get and print product's details
                                    case 'p':
                                        Console.WriteLine("Enter product's id");
                                        tempProductsID = Console.ReadLine();
                                        int.TryParse(tempProductsID, out productID);
                                        product = _iBl.Product.GetProductDetails(productID);
                                        Console.WriteLine(product);
                                        break;
                                    // get ant print catalog's product's details
                                    case 'c':
                                        Console.WriteLine("Enter product's id");
                                        tempProductsID = Console.ReadLine();
                                        int.TryParse(tempProductsID, out productID);
                                        productItem = _iBl.Product.GetProductFromCatalog(productID, cart);
                                        Console.WriteLine(productItem);
                                        break;
                                    // add a product
                                    case 'a':
                                        GetProductDetails(ref product);
                                        _iBl.Product.AddProduct(product);
                                        break;
                                    // delete a product
                                    case 'd':
                                        Console.WriteLine("Enter product's id");
                                        tempProductsID = Console.ReadLine();
                                        int.TryParse(tempProductsID, out productID);
                                        _iBl.Product.DeleteProduct(productID);
                                        break;
                                    // update product's details
                                    case 'u':
                                        GetProductDetails(ref product);
                                        _iBl.Product.UpdateProduct(product);
                                        break;
                                    // return to main menue
                                    case 'e':
                                        break;
                                    default: break;

                                }
                            }
                            while (secondChoise != 'e');
                        }
                        catch (blExceptions ex)
                        {
                            catchErrors(ex);
                        }

                        break;
                    // cart's operations
                    case 3:
                        try
                        {
                            do
                            {
                                Console.WriteLine("a. add produc to cart\nu. update product's amount\np. place order\ne. return to main menue");
                                tempSecondChoise = Console.ReadLine();
                                char.TryParse(tempSecondChoise, out secondChoise);
                                switch (secondChoise)
                                {
                                    // add a product to cart
                                    case 'a':
                                        Console.WriteLine("Enter product's id");
                                        tempProductsID = Console.ReadLine();
                                        int.TryParse(tempProductsID, out productID);
                                        product = _iBl.Product.GetProductDetails(productID);
                                        _iBl.Cart.AddProductToCart(cart, productID);
                                        break;
                                    // update product in cart
                                    case 'u':
                                        Console.WriteLine("Enter product's id");
                                        tempProductsID = Console.ReadLine();
                                        int.TryParse(tempProductsID, out productID);
                                        product = _iBl.Product.GetProductDetails(productID);
                                        int amount;
                                        Console.WriteLine("Enter product's new amount");
                                        string tempAmount = Console.ReadLine();
                                        int.TryParse(tempAmount, out amount);
                                        _iBl.Cart.UpdateProductAmountInCart(cart, productID, amount);
                                        break;
                                    // make an order from the cart
                                    case 'p':
                                        getCartDetails(ref cart);
                                        _iBl.Cart.PlaceOrder(cart);
                                        break;
                                    // return to main menue
                                    case 'e':
                                        break;
                                    default: break;
                                }
                            }
                            while (secondChoise != 'e');
                        }
                        catch (blExceptions ex)
                        {
                            catchErrors(ex);
                        }

                        break;
                }
            }
            while (firstChoice != 0);
        }
    }
}