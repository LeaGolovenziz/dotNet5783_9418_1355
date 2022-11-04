using Dal;
using DO;
using System;
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
        private static DalOrder dalOrder = new DalOrder();
        private static DalProduct dalProduct = new DalProduct();
        private DalOrderItem dalOrderItem = new DalOrderItem();

        public static Order GetOrder()
        {
            Order order = new Order();
            Console.WriteLine("enter costumer name:");
            string costumerName = Console.ReadLine();
            if (!costumerName.Equals(""))
                order.CustumerName = costumerName;
            Console.WriteLine("enter costumer Email:");
            string costumerEmail= Console.ReadLine();
            if (!costumerEmail.Equals(""))
                order.CustumerEmail = costumerEmail;
            Console.WriteLine("enter costumer adress:");
            string costumerAdress = Console.ReadLine();
            if (!costumerAdress.Equals(""))
                order.CustumerAdress = costumerAdress;
            try
            {
                Console.WriteLine("enter order's date in a dd.mm.yy format:");
                DateTime dateTime= DateTime.Parse(Console.ReadLine());
                if (dateTime != new DateTime())
                    order.OrderDate = dateTime;
                Console.WriteLine("enter order's ship date in a dd.mm.yy format:");
                dateTime = DateTime.Parse(Console.ReadLine());
                if (dateTime != new DateTime())
                    order.ShipDate = dateTime;
                Console.WriteLine("enter order's delivary date in a dd.mm.yy format:");
                dateTime = DateTime.Parse(Console.ReadLine());
                if (dateTime != new DateTime())
                    order.DeliveryDate = dateTime;
            }
            catch (Exception e)
            {
                Console.WriteLine("unvalid date value");
                return new Order();
            }
            return order;
        }
        static void Main()
        {
            Product InputProductDetails()
            {
                Product product = new Product();
                int id = 0;
                double price = 0;
                int inStock = 0;

                Console.WriteLine("Enter product's id:");
                id = int.Parse(Console.ReadLine());
                if(id!=0)
                    product.ID = id;

                Console.WriteLine("Enter product's name:");
                string name = Console.ReadLine();
                if(name!=" ")
                    product.Name = name;

                Console.WriteLine("Enter product's price:");
                price = double.Parse(Console.ReadLine());
                if(price!=0)
                    product.Price = price;

                Console.WriteLine("Enter product's category:");
                string category1 = Console.ReadLine();
                Enums.Category category = (Enums.Category)Convert.ToInt32(category1);
                if(category1!=" ")
                    product.Category = category;

                Console.WriteLine("Enter product's amount in stock:");
                inStock = int.Parse(Console.ReadLine());
                if(inStock!=0)
                    product.InStock = inStock;

                return product;
            }
            void InputIDAndprintProduct()
            {
                Product product;

                Console.WriteLine("Enter product's id");
                int id = int.Parse(Console.ReadLine());

                product = dalProduct.Get(id);
                Console.WriteLine(product.ToString());
            }

            int firstChoice = 0;

            //Console.WriteLine("1. check order\n2. check product\n3. check order's item check\n0. to exit\n");
            //int firstChoice = int.Parse(Console.ReadLine());
            //DateTime dateTime = DateTime.Parse(Console.ReadLine());
            do
            {
                try
                {
                    Console.WriteLine("1. check order\n2. check product\n3. check order's item \n0. to exit\n");
                    firstChoice = int.Parse(Console.ReadLine());
                    switch (firstChoice)
                    {
                        case 0:
                            Console.WriteLine("bye");
                            break;
                        case 1:
                            Console.WriteLine("a. add order\nb. update product\nc. get order\nd. get all orders\ne. delete orders");
                            char secondChoise = char.Parse(Console.ReadLine());
                            Order order;
                            int orderID;
                            switch (secondChoise)
                            {
                                case 'a':
                                    order = GetOrder();
                                    if (order.CustumerName.Equals(""))
                                        break;
                                    dalOrder.Add(order);
                                    break;
                                case 'b':
                                    Console.WriteLine("ender order id:");
                                    orderID = int.Parse(Console.ReadLine());
                                    order = dalOrder.Get(orderID);
                                    Console.WriteLine(order.ToString());
                                    order = GetOrder();
                                    order.ID = orderID;
                                    dalOrder.Update(order);
                                    break;
                                case 'c':
                                    Console.WriteLine("ender order id:");
                                    orderID = int.Parse(Console.ReadLine());
                                    order = dalOrder.Get(orderID);
                                    Console.WriteLine(order.ToString());
                                    break;
                                case 'd':
                                    List<Order> lstOrders = dalOrder.Get();
                                    //print?
                                    break;
                                case 'e':
                                    Console.WriteLine("ender order id:");
                                    orderID = int.Parse(Console.ReadLine());
                                    dalOrder.Delete(orderID);
                                    break;

                            default: break;
                        }
                        break;
                    case 2:
                        Console.WriteLine("a. add product\nb. update product\nc. get product\nd. get all products\ne. delete product");
                        secondChoise = char.Parse(Console.ReadLine());
                        Product product;
                        switch (secondChoise)
                        {
                            case 'a':
                                product = InputProductDetails();
                                Console.WriteLine(dalProduct.Add(product));
                                break;

                            case 'b':
                                InputIDAndprintProduct();
                                product = InputProductDetails();
                                dalProduct.Update(product);
                                break;

                            case 'c':
                                InputIDAndprintProduct();
                                break;

                            case 'd':
                                break;

                            case 'e':
                                Console.WriteLine("Enter product's id");
                                int id = int.Parse(Console.ReadLine());
                                dalProduct.Delete(id);

                                break;
                        }
                        break;

                    case 3:
                        break;

                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            while (firstChoice != 0);

        }
    }
}