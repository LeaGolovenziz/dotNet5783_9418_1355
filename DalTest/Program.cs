using Dal;
using DalApi;
using DO;
using static DO.Enums;

namespace DalTest
{
    public class Program
    {
        private static IDal _iDal = new DalList();

        void printEntity(Type t)
        {
            Type type = t.GetType();

        }

        static void Main()
        {
            // input a dateTime untill input value is valid
            DateTime badDateTime()
            {
                Console.WriteLine("bad date input, try again");
                string dateTime1 = Console.ReadLine()!;
                DateTime dateTime;
                if (!dateTime1.Equals(""))
                {
                    if (DateTime.TryParse(dateTime1, out dateTime))
                        return dateTime;
                    return badDateTime();
                }
                return DateTime.MinValue;
            }

            // input a int untill input value is valid
            int badInt()
            {
                Console.WriteLine("bad int input, try again");
                string int1 = Console.ReadLine()!;
                int i;
                if (int.TryParse(int1, out i))
                    return i;
                return badInt();
            }

            // input a double untill input value is valid
            double badDouble()
            {
                Console.WriteLine("bad int input, try again");
                string double1 = Console.ReadLine()!;
                double d;
                if (double.TryParse(double1, out d))
                    return d;
                return badDouble();
            }

            // input a int untill input value is valid
            char badChar()
            {
                Console.WriteLine("bad char input, try again");
                string char1 = Console.ReadLine()!;
                char c;
                if (char.TryParse(char1, out c))
                    return c;
                return badChar();
            }


            // input a category untill input value is valid
            Category badCategory()
            {
                Console.WriteLine("bad category input, try again");
                string category1 = Console.ReadLine()!;
                Category c;
                if (Category.TryParse(category1, out c))
                    return c;
                return badCategory();
            }

            // inputs order's details, creates an order and returns it
            Order InputOrderDetails(ref Order order)
            {
                DateTime dateTime;
                string dateTime1;

                Console.WriteLine("enter costumer name:");
                string costumerName = Console.ReadLine()!;
                if (!costumerName.Equals(""))
                    order.CustomerName = costumerName;

                Console.WriteLine("enter costumer Email:");
                string costumerEmail = Console.ReadLine()!;
                if (!costumerEmail.Equals(""))
                    order.CustomerEmail = costumerEmail;

                Console.WriteLine("enter costumer adress:");
                string costumerAdress = Console.ReadLine()!;
                if (!costumerAdress.Equals(""))
                    order.CustomerAdress = costumerAdress;

                Console.WriteLine("enter order's date in a dd.mm.yy format:");
                dateTime1 = Console.ReadLine()!;
                if (!dateTime1.Equals(""))
                {
                    if (DateTime.TryParse(dateTime1, out dateTime))
                        order.OrderDate = dateTime;
                    order.OrderDate = badDateTime();
                }

                Console.WriteLine("enter order's ship date in a dd.mm.yy format:");
                dateTime1 = Console.ReadLine()!;
                if (!dateTime1.Equals(""))
                {
                    if (DateTime.TryParse(dateTime1, out dateTime))
                        order.ShipDate = dateTime;
                    order.ShipDate = badDateTime();
                }

                Console.WriteLine("enter order's delivary date in a dd.mm.yy format:");
                dateTime1 = Console.ReadLine()!;
                if (!dateTime1.Equals(""))
                {
                    if (DateTime.TryParse(dateTime1, out dateTime))
                        order.DeliveryDate = dateTime;
                    order.DeliveryDate = badDateTime();
                }

                return order;
            }
            void inputIDAndPrintOrder(ref Order order)
            {
                // input id
                Console.WriteLine("enter order id:");
                string tempOrderID = Console.ReadLine()!;
                int orderID;
                if (!int.TryParse(tempOrderID, out orderID))
                    orderID = badInt();

                // print the order with "orderID" ID
                order = _iDal.Order.Get(orderID);
                Console.WriteLine(order.ToString());
            }

            // gets product that have an ID, inputs its other details and returns it
            Product GetProductDetails(ref Product product)
            {
                Console.WriteLine("Enter product's name:");
                string name = Console.ReadLine()!;
                if (name != "")
                    product.Name = name;

                Console.WriteLine("Enter product's price:");
                string tempPrice = Console.ReadLine()!;
                double price;
                if (!double.TryParse(tempPrice, out price))
                    price = badDouble();
                if (price != 0)
                    product.Price = price;

                Console.WriteLine("Enter product's category:");
                string category1 = Console.ReadLine()!;
                Enums.Category category;
                if (category1 != "")
                {
                    if (!Enums.Category.TryParse(category1, out category))
                        product.Category = badCategory();
                    product.Category = category;
                }

                Console.WriteLine("Enter product's amount in stock:");
                string tempInStock = Console.ReadLine()!;
                int inStock;
                if (!int.TryParse(tempInStock, out inStock))
                    inStock = badInt();
                if (inStock != 0)
                    product.InStock = inStock;

                return product;
            }
            // gets produt, ask id from the user and print and return the prodect
            void inputIDAndPrintProduct(ref Product product)
            {
                // input ID
                Console.WriteLine("Enter product's id:");
                string tempProductsID = Console.ReadLine()!;
                int ProductsID;
                if (!int.TryParse(tempProductsID, out ProductsID))
                    ProductsID = badInt();
                product.ID = ProductsID;
                // get the product with this ID and print it 
                product = _iDal.Product.Get(product.ID);
                Console.WriteLine(product.ToString());
            }

            // inputs order's item details, creates an order's item and returns it
            OrderItem GetOrderItemDetails(ref OrderItem orderItem)
            {
                Console.WriteLine("enter product's ID:");
                string tempProductsID = Console.ReadLine()!;
                int ProductsID;
                if (!int.TryParse(tempProductsID, out ProductsID))
                    ProductsID = badInt();
                if (ProductsID != 0)
                    orderItem.ID = ProductsID;

                Console.WriteLine("enter order's ID:");
                string tempOrderID = Console.ReadLine()!;
                int orderID;
                if (!int.TryParse(tempOrderID, out orderID))
                    orderID = badInt();
                if (orderID != 0)
                    orderItem.OrderID = orderID;

                Console.WriteLine("enter product's price:");
                string tempPrice = Console.ReadLine()!;
                double price;
                if (!double.TryParse(tempPrice, out price))
                    price = badDouble();
                if (price != 0)
                    orderItem.Price = price;

                Console.WriteLine("enter product's amount:");
                string tempAmount = Console.ReadLine()!;
                int amount;
                if (!int.TryParse(tempAmount, out amount))
                    amount = badInt();
                if (amount != 0)
                    orderItem.ProductAmount = amount;

                return orderItem;
            }
            // gets order's item, ask id from user and print and return the order's id
            void InputOrderItemIDAndPrint(ref OrderItem orderItem)
            {
                Console.WriteLine("enter order's item id:");
                string tempOrdersID = Console.ReadLine()!;
                int ordersID;
                if (!int.TryParse(tempOrdersID, out ordersID))
                    ordersID = badInt();
                orderItem = _iDal.OrderItem.Get(orderItem.OrderItemID);
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
                    string tempFirstChoice = Console.ReadLine()!;
                    if (!int.TryParse(tempFirstChoice, out firstChoice))
                        firstChoice = badInt();

                    switch (firstChoice)
                    {
                        // exit
                        case 0:
                            Console.WriteLine("bye");
                            break;
                        // order operations
                        case 1:
                            Console.WriteLine("a. add order\nb. update product\nc. get order\nd. get all orders\ne. delete orders");
                            char secondChoise;
                            string tempSecondChoice = Console.ReadLine()!;
                            if (!char.TryParse(tempSecondChoice, out secondChoise))
                                secondChoise = badChar();

                            switch (secondChoise)
                            {
                                // add order
                                case 'a':
                                    InputOrderDetails(ref order);

                                    Console.WriteLine(_iDal.Order.Add(order));

                                    break;
                                // update order
                                case 'b':
                                    inputIDAndPrintOrder(ref order);

                                    // input the other details of order
                                    InputOrderDetails(ref order);

                                    // update the order in the list
                                    _iDal.Order.Update(order);

                                    break;

                                case 'c':
                                    inputIDAndPrintOrder(ref order);

                                    break;

                                case 'd':
                                    // gets ienumerable of the list 
                                    IEnumerator<Order?> ieOrders = _iDal.Order.Get().GetEnumerator();
                                    // prints the list
                                    while (ieOrders.MoveNext())
                                    {
                                        order = (Order)ieOrders.Current!;
                                        Console.WriteLine(order.ToString());
                                    }

                                    break;

                                case 'e':
                                    Console.WriteLine("enter order id:");

                                    string tempOrderID = Console.ReadLine()!;
                                    if (!int.TryParse(tempOrderID, out orderID))
                                        orderID = badChar();

                                    // deletes an order from the list
                                    _iDal.Order.Delete(orderID);

                                    break;
                            }

                            break;
                        // product operations
                        case 2:
                            Console.WriteLine("a. add product\nb. update product\nc. get product\nd. get all products\ne. delete product");
                            tempSecondChoice = Console.ReadLine()!;
                            if (!char.TryParse(tempSecondChoice, out secondChoise))
                                secondChoise = badChar();

                            switch (secondChoise)
                            {
                                // add product
                                case 'a':
                                    Console.WriteLine("Enter product's id:");
                                    string tempProductID = Console.ReadLine()!;
                                    if (!int.TryParse(tempProductID, out productID))
                                        productID = badChar();
                                    product.ID = productID;
                                    // input the other 
                                    product = GetProductDetails(ref product);
                                    // add product to the list
                                    Console.WriteLine(_iDal.Product.Add(product));
                                    break;
                                // update product's details 
                                case 'b':
                                    inputIDAndPrintProduct(ref product);

                                    // input the product details of product
                                    GetProductDetails(ref product);

                                    // update the product in the list
                                    _iDal.Product.Update(product);

                                    break;

                                case 'c':
                                    inputIDAndPrintProduct(ref product);
                                    break;

                                case 'd':
                                    // gets ienumerable of the list
                                    IEnumerator<Product?> ieProduct = _iDal.Product.Get().GetEnumerator();
                                    //print the lis
                                    while (ieProduct.MoveNext())
                                    {
                                        product = (Product)ieProduct.Current!;
                                        Console.WriteLine(product.ToString());
                                    }
                                    break;

                                case 'e':
                                    Console.WriteLine("Enter product's id");
                                    string tempID = Console.ReadLine()!;
                                    int id;
                                    if (!int.TryParse(tempID, out id))
                                        id = badInt();

                                    // delete the product from the list
                                    _iDal.Product.Delete(id);

                                    break;
                            }

                            break;
                        // order's items operations
                        case 3:
                            Console.WriteLine("a. add order's item\nb. update order's item\nc. get order's item\nd. get all order's item\ne. delete order's item\nf.get order's item by an order and a item\ng.get all order's item by order");
                            tempSecondChoice = Console.ReadLine()!;
                            if (!char.TryParse(tempSecondChoice, out secondChoise))
                                secondChoise = badChar();

                            switch (secondChoise)
                            {
                                // add order item
                                case 'a':
                                    GetOrderItemDetails(ref orderItem);

                                    Console.WriteLine(_iDal.OrderItem.Add(orderItem));

                                    break;
                                // update order item
                                case 'b':
                                    InputOrderItemIDAndPrint(ref orderItem);

                                    GetOrderItemDetails(ref orderItem);

                                    _iDal.OrderItem.Update(orderItem);

                                    break;
                                // print certain order item - by id
                                case 'c':
                                    InputOrderItemIDAndPrint(ref orderItem);

                                    break;
                                // print all order items
                                case 'd':
                                    IEnumerable<OrderItem?> ieOrderItems = _iDal.OrderItem.Get();
                                    foreach (OrderItem? oi in ieOrderItems)
                                    {
                                        Console.WriteLine(oi.ToString());
                                    }

                                    break;
                                // delete order item
                                case 'e':
                                    Console.WriteLine("enter order's item id:");
                                    string tepOrderItemID = Console.ReadLine()!;
                                    if (!int.TryParse(tepOrderItemID, out orderItemID))
                                        orderItemID = badInt();
                                    _iDal.OrderItem.Delete(orderItemID);

                                    break;
                                // print certain order item - by product and order
                                case 'f':
                                    Console.WriteLine("enter product's id:");
                                    string tempProductID = Console.ReadLine()!;
                                    if (!int.TryParse(tempProductID, out productID))
                                        productID = badInt();
                                    Console.WriteLine("enter order's id:");
                                    string tempOrderID = Console.ReadLine()!;
                                    if (!int.TryParse(tempOrderID, out orderID))
                                        orderID = badInt();

                                    // gets the order's item
                                    orderItem = _iDal.OrderItem.Get(productID, orderID);

                                    // prints the order's item
                                    Console.WriteLine(orderItem.ToString());

                                    break;

                                // get and print the list of products by order's id
                                case 'g':
                                    Console.WriteLine("enter order's id:");
                                    tempOrderID = Console.ReadLine()!;
                                    if (!int.TryParse(tempOrderID, out orderID))
                                        orderID = badInt();

                                    // gets ienumerable of the list
                                    IEnumerable<OrderItem?> ieItems = _iDal.OrderItem.GeOrderItems(orderID);

                                    // prints the list
                                    foreach (OrderItem? oi in ieItems)
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