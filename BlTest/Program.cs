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

namespace BlTest
{
    internal class Program
    {
        private static IBl _iBl = new Bl();

        static void Main()
        {
            // inputs order's details, creates an order and returns it
            //Order InputOrderDetails(ref Order order)
            //{
            //    DateTime dateTime;
            //    string dateTime1;

            //    Console.WriteLine("enter costumer name:");
            //    string costumerName = Console.ReadLine();
            //    if (!costumerName.Equals(""))
            //        order.CustomerName = costumerName;

            //    Console.WriteLine("enter costumer Email:");
            //    string costumerEmail = Console.ReadLine();
            //    if (!costumerEmail.Equals(""))
            //        order.CustomerEmail = costumerEmail;

            //    Console.WriteLine("enter costumer adress:");
            //    string costumerAdress = Console.ReadLine();
            //    if (!costumerAdress.Equals(""))
            //        order.CustomerAdress = costumerAdress;

            //    Console.WriteLine("enter order's date in a dd.mm.yy format:");
            //    dateTime1 = Console.ReadLine();
            //    if (dateTime1.Equals(""))
            //    {
            //        DateTime.TryParse(dateTime1, out dateTime);
            //        order.OrderDate = dateTime;
            //    }

            //    Console.WriteLine("enter order's ship date in a dd.mm.yy format:");
            //    dateTime1 = Console.ReadLine();
            //    if (dateTime1.Equals(""))
            //    {
            //        DateTime.TryParse(dateTime1, out dateTime);
            //        order.ShipDate = dateTime;
            //    }

            //    Console.WriteLine("enter order's delivary date in a dd.mm.yy format:");
            //    dateTime1 = Console.ReadLine();
            //    if (dateTime1.Equals(""))
            //    {
            //        DateTime.TryParse(dateTime1, out dateTime);
            //        order.DeliveryDate = dateTime;
            //    }

            //    return order;
            //}
            //void inputIDAndPrintOrder(ref Order order)
            //{
            //    // input id
            //    Console.WriteLine("enter order id:");
            //    int orderID = int.Parse(Console.ReadLine());
            //    // print the order with "orderID" ID
            //    order = _iDal.Order.Get(orderID);
            //    Console.WriteLine(order.ToString());
            //}

            //// gets product that have an ID, inputs its other details and returns it
            //Product GetProductDetails(ref Product product)
            //{
            //    Console.WriteLine("Enter product's name:");
            //    string name = Console.ReadLine();
            //    if (name != " ")
            //        product.Name = name;

            //    Console.WriteLine("Enter product's price:");
            //    double price = double.Parse(Console.ReadLine());
            //    if (price != 0)
            //        product.Price = price;

            //    Console.WriteLine("Enter product's category:");
            //    string category1 = Console.ReadLine();
            //    Enums.Category category = (Enums.Category)Enum.Parse(typeof(Enums.Category), category1);
            //    if (category1 != " ")
            //        product.Category = category;

            //    Console.WriteLine("Enter product's amount in stock:");
            //    string tempInStock = Console.ReadLine();
            //    int inStock;
            //    int.TryParse(tempInStock, out inStock);
            //    if (inStock != 0)
            //        product.InStock = inStock;

            //    return product;
            //}
            //// gets produt, ask id from the user and print and return the prodect
            //void inputIDAndPrintProduct(ref Product product)
            //{
            //    // input ID
            //    Console.WriteLine("Enter product's id:");
            //    string tempProductsID = Console.ReadLine();
            //    int ProductsID;
            //    int.TryParse(tempProductsID, out ProductsID);
            //    product.ID = int.Parse(Console.ReadLine());
            //    // get the product with this ID and print it 
            //    product = _iDal.Product.Get(product.ID);
            //    Console.WriteLine(product.ToString());
            //}

            //// inputs order's item details, creates an order's item and returns it
            //OrderItem GetOrderItemDetails(ref OrderItem orderItem)
            //{
            //    Console.WriteLine("enter product's ID:");
            //    string tempProductsID = Console.ReadLine();
            //    int ProductsID;
            //    int.TryParse(tempProductsID, out ProductsID);
            //    int productID = ProductsID;
            //    if (productID != 0)
            //        orderItem.ProductID = productID;

            //    Console.WriteLine("enter order's ID:");
            //    string tempOrderID = Console.ReadLine();
            //    int orderID;
            //    int.TryParse(tempOrderID, out orderID);
            //    if (orderID != 0)
            //        orderItem.OrderID = orderID;

            //    Console.WriteLine("enter product's price:");
            //    string tempPrice = Console.ReadLine();
            //    double price;
            //    double.TryParse(tempPrice, out price);
            //    if (price != 0)
            //        orderItem.Price = price;

            //    Console.WriteLine("enter product's amount:");
            //    string tempAmount = Console.ReadLine();
            //    int amount;
            //    int.TryParse(tempAmount, out amount);
            //    if (amount != 0)
            //        orderItem.Amount = amount;

            //    return orderItem;
            //}
            //// gets order's item, ask id from user and print and return the order's id
            //void InputOrderItemIDAndPrint(ref OrderItem orderItem)
            //{
            //    Console.WriteLine("enter order's item id:");
            //    orderItem.OrderItemID = int.Parse(Console.ReadLine());
            //    orderItem = _iDal.OrderItem.Get(orderItem.OrderItemID);
            //    Console.WriteLine(orderItem.ToString());
            //}

            int firstChoice = 0, cartID, orderID, productID;
            Order order = new Order();
            Product product = new Product();
            Cart cart = new Cart();

            do
            {
                try
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
                                        foreach(OrderForList orderInList in lst)
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

                                        OrderTracking orderT= _iBl.Order.TrackOrder(orderID);

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
                                        int.TryParse(tempOrderID, out productID);
                                        Console.WriteLine("Enter new amount");
                                        tempNewAmount = Console.ReadLine();
                                        int.TryParse(tempNewAmount, out newAmount);

                                        order = _iBl.Order.UpdateOrderDetails(orderID, productID, newAmount);

                                        Console.WriteLine(order);
                                        
                                        break;
                                }
                            }
                            while (!secondChoise.Equals('e'));
                            
                            break;
                        // product operations
                        case 2:
                            Console.WriteLine("g. get products' list\np. get product's details\nc. get product from catalog\na. add product\nd. delete product\nu. update product's details\ne. return to main menue");
                            tempSecondChoise = Console.ReadLine();
                            char.TryParse(tempSecondChoise, out secondChoise);

                            switch (secondChoise)
                            {
                                
                            }

                            break;
                        // cart's operations
                        case 3:
                            Console.WriteLine("a. add produc to cart\nu. update product's amount\np. place order\ne. return to main menue");
                            tempSecondChoise = Console.ReadLine();
                            char.TryParse(tempSecondChoise, out secondChoise);

                            switch (secondChoise)
                            {
                                
                            }
                            break;
                    }
                }
                catch (DoesntExist ex)
                {
                    
                }
            }
            while (firstChoice != 0);
        }
    }
}