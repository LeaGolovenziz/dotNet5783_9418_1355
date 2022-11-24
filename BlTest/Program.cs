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
using System.Collections.Specialized;

namespace BlTest
{
    internal class Program
    {
        private static IBl _iBl = new Bl();

        static void Main()
        {
            Cart getCartDetails(ref Cart cart)
            {
                Console.WriteLine("Enter customer's name:");
                string name = Console.ReadLine();
                cart.CustomerName= name;

                Console.WriteLine("Enter customer's email:");
                string email = Console.ReadLine();
                cart.CustomerEmail = email;

                Console.WriteLine("Enter customer's address:");
                string address = Console.ReadLine();
                cart.CustomerAddress = address;

                return cart;
            }

            // gets product that have an ID, inputs its other details and returns it
            Product GetProductDetails(ref Product product)
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

                return product;
            }

            int firstChoice = 0, cartID, orderID, productID;
            Order order = new Order();
            Product product = new Product();
            Cart cart = new Cart();
            string tempProductsID;
            ProductItem productItem;

            do
            {
                try
                {
                    Console.WriteLine("1. check order\n2. check product\n3. check cart \n0. to exit\n");
                    firstChoice = int.Parse(Console.ReadLine());

                    switch (firstChoice)
                    {
                        // exit
                        case 0:
                            Console.WriteLine("bye");
                            break;
                        // order operations
                        case 1:
                            Console.WriteLine("d. update order's delivery\ns. update order's shipping\ng. get orders' list\no. get order's details\nt. track order\nu. update order's details\ne. to return to main menue\n");
                            string tempSecondChoise = Console.ReadLine();
                            char secondChoise;
                            char.TryParse(tempSecondChoise, out secondChoise);

                            switch (secondChoise)
                            {
                                
                            }

                            break;
                        // product operations
                        case 2:
                            Console.WriteLine("g. get products' list\np. get product's details\nc. get product from catalog\na. add product\nd. delete product\nu. update product's details\ne. return to main menue\n");
                            tempSecondChoise = Console.ReadLine();
                            char.TryParse(tempSecondChoise, out secondChoise);

                            do
                            {
                                switch (secondChoise)
                                {
                                    case 'g':
                                        IEnumerable<ProductForList> products = _iBl.Product.GetProductsList();
                                        foreach(ProductForList productForList in products)
                                        {
                                            Console.WriteLine(productForList);
                                        }
                                        break;
                                    case 'p':
                                        tempProductsID = Console.ReadLine();
                                        int.TryParse(tempProductsID, out productID);
                                        product = _iBl.Product.GetProductDetails(productID);
                                        Console.WriteLine(product);
                                        break;
                                    case 'c':
                                        tempProductsID = Console.ReadLine();
                                        int.TryParse(tempProductsID, out productID);
                                        productItem = _iBl.Product.GetProductFromCatalog(productID,cart);
                                        Console.WriteLine(productItem);
                                        break;
                                    case 'a':
                                        GetProductDetails(ref product);
                                        _iBl.Product.AddProduct(product);
                                        break;
                                    case 'd':
                                        tempProductsID = Console.ReadLine();
                                        int.TryParse(tempProductsID, out productID);
                                        _iBl.Product.DeleteProduct(productID);
                                        break;
                                    case 'u':
                                        GetProductDetails(ref product);
                                        _iBl.Product.UpdateProduct(product);
                                        break;
                                    case 'e':
                                        break;
                                        default: break;

                                }
                                char.TryParse(tempSecondChoise, out secondChoise);
                            }
                            while (secondChoise != 'e');

                            break;
                        // cart's operations
                        case 3:
                            Console.WriteLine("a. add produc to cart\nu. update product's amount\np. place order\ne. return to main menue");
                            tempSecondChoise = Console.ReadLine();
                            char.TryParse(tempSecondChoise, out secondChoise);

                            do
                            {
                                switch (secondChoise)
                                {
                                    case 'a':
                                        tempProductsID = Console.ReadLine();
                                        int.TryParse(tempProductsID, out productID);
                                        product = _iBl.Product.GetProductDetails(productID);
                                        _iBl.Cart.AddProductToCart(cart, productID);
                                        break;
                                    case 'u':
                                        tempProductsID = Console.ReadLine();
                                        int.TryParse(tempProductsID, out productID);
                                        product = _iBl.Product.GetProductDetails(productID);
                                        int amount;
                                        string tempAmount = Console.ReadLine();
                                        int.TryParse(tempAmount, out amount);
                                        _iBl.Cart.UpdateProductAmountInCart(cart, productID,amount);
                                        break;
                                    case 'p':
                                        getCartDetails(ref cart);
                                        _iBl.Cart.PlaceOrder(cart, cart.CustomerName, cart.CustomerEmail, cart.CustomerAddress);
                                        break;
                                    case 'e':
                                        break;
                                    default:break;
                                }
                                char.TryParse(tempSecondChoise, out secondChoise);
                            }
                            while (secondChoise != 'e');
                            break;
                    }
                }
                catch (blExceptions ex)
                {
                    Console.WriteLine(ex.GetType().Name);
                    Console.WriteLine(ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine(ex.InnerException.GetType().Name);
                        Console.WriteLine(ex.InnerException.Message);
                    }
                }
            }
            while (firstChoice != 0);
        }
    }
}