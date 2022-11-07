using Dal;
using DO;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using static DO.Enums;
using System.Xml.Linq;

namespace DalTest
{
    public class Program
    {
        private static DalOrder _dalOrder = new DalOrder();
        private static DalProduct _dalProduct = new DalProduct();
        private static DalOrderItem _dalOrderItem = new DalOrderItem();

        static void Main()
        {
            // inputs order's details, creates an order and returns it
            Order InputOrderDetails(ref Order order)
            {
                DateTime dateTime;
                string dateTime1;

                Console.WriteLine("enter costumer name:");
                string costumerName = Console.ReadLine();
                if (!costumerName.Equals(""))
                    order.CustumerName = costumerName;

                Console.WriteLine("enter costumer Email:");
                string costumerEmail = Console.ReadLine();
                if (!costumerEmail.Equals(""))
                    order.CustumerEmail = costumerEmail;

                Console.WriteLine("enter costumer adress:");
                string costumerAdress = Console.ReadLine();
                if (!costumerAdress.Equals(""))
                    order.CustumerAdress = costumerAdress;

                Console.WriteLine("enter order's date in a dd.mm.yy format:");
                dateTime1 = Console.ReadLine();
                if (dateTime1.Equals(""))
                {
                    DateTime.TryParse(dateTime1, out dateTime);
                    order.OrderDate = dateTime;
                }

                Console.WriteLine("enter order's ship date in a dd.mm.yy format:");
                dateTime1 = Console.ReadLine();
                if (dateTime1.Equals(""))
                {
                    DateTime.TryParse(dateTime1, out dateTime);
                    order.ShipDate = dateTime;
                }

                Console.WriteLine("enter order's delivary date in a dd.mm.yy format:");
                dateTime1 = Console.ReadLine();
                if (dateTime1.Equals(""))
                {
                    DateTime.TryParse(dateTime1, out dateTime);
                    order.DeliveryDate = dateTime;
                }

                return order;
            }
            void inputIDAndPrintOrder(ref Order order)
            {
                // input id
                Console.WriteLine("enter order id:");
                int orderID = int.Parse(Console.ReadLine());
                // print the order with "orderID" ID
                order = _dalOrder.Get(orderID);
                Console.WriteLine(order.ToString());
            }

            // gets product that have an ID, inputs its other details and returns it
            Product GetProductDetails(ref Product product)
            {
                Console.WriteLine("Enter product's name:");
                string name = Console.ReadLine();
                if (name != " ")
                    product.Name = name;

                Console.WriteLine("Enter product's price:");
                double price = double.Parse(Console.ReadLine());
                if (price != 0)
                    product.Price = price;

                Console.WriteLine("Enter product's category:");
                string category1 = Console.ReadLine();
                Enums.Category category = (Enums.Category)Enum.Parse(typeof(Enums.Category), category1);
                if (category1 != " ")
                    product.Category = category;

                Console.WriteLine("Enter product's amount in stock:");
                string tempInStock= Console.ReadLine();
                int inStock; 
                int.TryParse(tempInStock,out inStock) ;
                if (inStock != 0)
                    product.InStock = inStock;

                return product;
            }
            // gets produt, ask id from the user and print and return the prodect
            void inputIDAndPrintProduct(ref Product product)
            {
                // input ID
                Console.WriteLine("Enter product's id:");
                string tempProductsID = Console.ReadLine();
                int ProductsID;
                int.TryParse(tempProductsID, out ProductsID);
                product.ID = int.Parse(Console.ReadLine());
                // get the product with this ID and print it 
                product = _dalProduct.Get(product.ID);
                Console.WriteLine(product.ToString());
            }

            // inputs order's item details, creates an order's item and returns it
            OrderItem GetOrderItemDetails(ref OrderItem orderItem)
            {
                Console.WriteLine("enter product's ID:");
                string tempProductsID = Console.ReadLine();
                int ProductsID;
                int.TryParse(tempProductsID, out ProductsID);
                int productID = ProductsID;
                if (productID != 0)
                    orderItem.ProductID = productID;

                Console.WriteLine("enter order's ID:");
                string tempOrderID = Console.ReadLine();
                int orderID;
                int.TryParse(tempOrderID, out orderID);
                if (orderID != 0)
                    orderItem.OrderID = orderID;

                Console.WriteLine("enter product's price:");
                string tempPrice = Console.ReadLine();
                double price;
                double.TryParse(tempPrice, out price);
                if (price != 0)
                    orderItem.Price = price;

                Console.WriteLine("enter product's amount:");
                string tempAmount = Console.ReadLine();
                int amount;
                int.TryParse(tempAmount, out amount);
                if (amount != 0)
                    orderItem.Amount = amount;

                return orderItem;
            }
            // gets order's item, ask id from user and print and return the order's id
            void InputOrderItemIDAndPrint(ref OrderItem orderItem)
            {
                Console.WriteLine("enter order's item id:");
                orderItem.OrderItemID = int.Parse(Console.ReadLine());
                orderItem = _dalOrderItem.Get(orderItem.OrderItemID);
                Console.WriteLine(orderItem.ToString());
            }

            int firstChoice = 0, orderItemID, orderID, productID;
            Order order = new Order();
            Product product = new Product();
            OrderItem orderItem = new OrderItem();

            do
            {
                try
                {
                    Console.WriteLine("1. check order\n2. check product\n3. check order's item \n0. to exit\n");
                    firstChoice = int.Parse(Console.ReadLine());

                    switch (firstChoice)
                    {
                        // exit
                        case 0:
                            Console.WriteLine("bye");
                            break;
                        // order operations
                        case 1:
                            Console.WriteLine("a. add order\nb. update product\nc. get order\nd. get all orders\ne. delete orders");
                            string tempSecondChoise = Console.ReadLine();
                            char secondChoise;
                            char.TryParse(tempSecondChoise, out secondChoise);

                            switch (secondChoise)
                            {
                                // add order
                                case 'a':
                                    InputOrderDetails(ref order);

                                    Console.WriteLine(_dalOrder.Add(order));

                                    break;
                                // update order
                                case 'b':
                                    inputIDAndPrintOrder(ref order);

                                    // input the other details of order
                                    InputOrderDetails(ref order);

                                    // update the order in the list
                                    _dalOrder.Update(order);

                                    break;

                                case 'c':
                                    inputIDAndPrintOrder(ref order);

                                    break;

                                case 'd':
                                    // gets ienumerable of the list 
                                    IEnumerator<Order> ieOrders = _dalOrder.Get().GetEnumerator();
                                    // prints the list
                                    while (ieOrders.MoveNext())
                                    {
                                        order = (Order)ieOrders.Current;
                                        Console.WriteLine(order.ToString());
                                    }

                                    break;

                                case 'e':
                                    Console.WriteLine("enter order id:");

                                    string tempOrderID = Console.ReadLine();
                                    int.TryParse(tempOrderID, out orderID);

                                    // deletes an order from the list
                                    _dalOrder.Delete(orderID);

                                    break;
                            }

                            break;
                        // product operations
                        case 2:
                            Console.WriteLine("a. add product\nb. update product\nc. get product\nd. get all products\ne. delete product");
                            tempSecondChoise = Console.ReadLine();
                            char.TryParse(tempSecondChoise, out secondChoise);

                            switch (secondChoise)
                            {
                                // add product
                                case 'a':
                                    Console.WriteLine("Enter product's id:");
                                    string tempProductID = Console.ReadLine();
                                    int.TryParse(tempProductID, out productID);
                                    product.ID = productID;
                                    // input the other 
                                    product = GetProductDetails(ref product);
                                    // add product to the list
                                    Console.WriteLine(_dalProduct.Add(product));
                                    break;
                                // update product's details 
                                case 'b':
                                    inputIDAndPrintProduct(ref product);

                                    // input the product details of product
                                    GetProductDetails(ref product);

                                    // update the product in the list
                                    _dalProduct.Update(product);

                                    break;

                                case 'c':
                                    inputIDAndPrintProduct(ref product);
                                    break;

                                case 'd':
                                    // gets ienumerable of the list
                                    IEnumerator<Product> ieProduct = _dalProduct.Get().GetEnumerator();
                                    //print the lis
                                    while (ieProduct.MoveNext())
                                    {
                                        product = (Product)ieProduct.Current;
                                        Console.WriteLine(product.ToString());
                                    }
                                    break;

                                case 'e':
                                    Console.WriteLine("Enter product's id");
                                    string tempID = Console.ReadLine();
                                    int id;
                                    int.TryParse(tempID, out id);

                                    // delete the product from the list
                                    _dalProduct.Delete(id);

                                    break;
                            }

                            break;
                        // order's items operations
                        case 3:
                            Console.WriteLine("a. add order's item\nb. update order's item\nc. get order's item\nd. get all order's item\ne. delete order's item\nf.get order's item by an order and a item\ng.get all order's item by order");
                            tempSecondChoise = Console.ReadLine();
                            char.TryParse(tempSecondChoise, out secondChoise);

                            switch (secondChoise)
                            {
                                // add order item
                                case 'a':
                                    GetOrderItemDetails(ref orderItem);

                                    Console.WriteLine(_dalOrderItem.Add(orderItem));

                                    break;
                                // update order item
                                case 'b':
                                    InputOrderItemIDAndPrint(ref orderItem);

                                    GetOrderItemDetails(ref orderItem);

                                    _dalOrderItem.Update(orderItem);

                                    break;
                                // print certain order item - by id
                                case 'c':
                                    InputOrderItemIDAndPrint(ref orderItem);

                                    break;
                                // print all order items
                                case 'd':
                                    IEnumerable<OrderItem> ieOrderItems = _dalOrderItem.Get();
                                    foreach (OrderItem oi in ieOrderItems)
                                    {
                                        Console.WriteLine(oi.ToString());
                                    }

                                    break;
                                // delete order item
                                case 'e':
                                    Console.WriteLine("enter order's item id:");
                                    string tepOrderItemID = Console.ReadLine();
                                    int.TryParse(tepOrderItemID, out orderItemID);
                                    _dalOrderItem.Delete(orderItemID);

                                    break;
                                // print certain order item - by product and order
                                case 'f':
                                    Console.WriteLine("enter product's id:");
                                    string tempProductID = Console.ReadLine();
                                    int.TryParse(tempProductID, out productID);
                                    Console.WriteLine("enter order's id:");
                                    string tempOrderID = Console.ReadLine();
                                    int.TryParse(tempOrderID, out orderID);

                                    // gets the order's item
                                    orderItem = _dalOrderItem.Get(productID, orderID);

                                    // prints the order's item
                                    Console.WriteLine(orderItem.ToString());

                                    break;

                                    // get and print the list of products by order's id
                                case 'g':
                                    Console.WriteLine("enter order's id:");
                                    tempOrderID = Console.ReadLine();
                                    int.TryParse(tempOrderID, out orderID);

                                    // gets ienumerable of the list
                                    IEnumerable<OrderItem> ieItems = _dalOrderItem.GeOrderItems(orderID);

                                    // prints the list
                                    foreach (OrderItem oi in ieItems)
                                    {
                                        Console.WriteLine(oi.ToString());
                                    }

                                    break;
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            while (firstChoice != 0);
        }
    }
}