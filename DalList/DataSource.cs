﻿
using DO;

namespace Dal;

internal static class DataSource
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly Random rand = new Random();

    internal static List<Order> _lstOreders = new List<Order>();
    internal static List<OrderItem> _lstOrderItems = new List<OrderItem>();
    internal static List<Product> _lstPruducts = new List<Product>();

    static DataSource()
    {
        s_Initialize();
    }
    private static void s_Initialize()
    {
        // array of product's possible names
        string[] productsNamesArray = { "product1", "product2", "product3", "product4", "product5", "product6", "product7", "product8", "product9", "product10" };

        string[] firstNames = { "Sara", "Rebeka", "Rachel", "Leah", "Naomi" };

        string[] lastNames = { "Cohen", "Levi", "Israel", "Goldenkoif", "Kachanelbuge" };

        string[] cities = { "Jerusalem", "Ramat Gan", "Bnei Brak", "Beit Shemesh", "Ashdod" };

        string[] streets = { "Ben Guryon", "Habrosh", "Hazait", "Vaitzman", "Begin" };

        for (int i = 0; i < 10; i++)
        {
            // create new product
            Product product = new Product();

            // draws an id while there's alredy a product in the list with the same id
            do
            {
                product.ID = rand.Next(100000, 999999);
            }
            while (_lstPruducts.Exists(x => x.ID == product.ID));

            // select the name of the product from the names array
            product.Name = productsNamesArray[i];
            // draw the price of the product
            product.Price = (double)rand.Next(50, 200);
            // the stock is 0 the  first time
            product.InStock = i;
            //draw the category of the froduct
            product.Category = (Enums.Category)rand.Next(0, 4);

            // add the product to the list
            _lstPruducts.Add(product);
        }

        for (int i = 0; i < 20; i++)
        {
            // create new order
            Order order = new Order();

            // gets the next available id
            order.ID = config._OrderID;
            // draw a name and last name from the names and last names arrays
            string custumerFirstName = firstNames[rand.Next(0, 4)];
            string custumerLastName = lastNames[rand.Next(0, 4)];

            order.CustumerName = custumerFirstName + " " + custumerLastName;
            order.CustumerEmail = custumerFirstName + custumerLastName + "@gmail.com";
            // draw a city and a street fron the cities and streeats arrays
            order.CustumerAdress = streets[rand.Next(0, 4)] + " " + rand.Next(1, 100) + " " + cities[rand.Next(0, 4)];
            // draw a date in the rang between last year and two months ago
            order.OrderDate = DateTime.Now.AddMonths(rand.Next(-12, -2));

            //TimeSpan timeSpan = (TimeSpan)(order.OrderDate.Value.AddMonths(2)-order.OrderDate) ;

            // draw a date in the range between the order date and 7 days after
            order.ShipDate = order.OrderDate.Value.AddDays(rand.Next(7, 21));
            // draw a date in the range between the ship date and 2 days after
            order.DeliveryDate = order.ShipDate.Value.AddDays(rand.Next(2, 6));

            _lstOreders.Add(order);
        }
        for(int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                OrderItem orderItem = new OrderItem();

                orderItem.OrderItemID = config._ItemOrderID;
                Product p = _lstPruducts[rand.Next(0, 9)];
                orderItem.ProductID = p.ID;
                orderItem.Price = p.Price;
                orderItem.OrderID = 100000 + i;
                orderItem.Amount = rand.Next(1, 10);

                _lstOrderItems.Add(orderItem);
            }
        }

    }

    private static void addOrder(Order or)
    {
        _lstOreders.Add(or);
    }
    private static void addProduct(Product pro)
    {
        _lstPruducts.Add(pro);
    }
    private static void addOrderedItem(OrderItem orit)
    {
        _lstOrderItems.Add(orit);
    }

    internal static class config
    {
        private static int _itemOrderID = 100000;
        private static int _orderID = 100000;

        public static int _ItemOrderID
        {
            get { return _itemOrderID++; }
        }

        public static int _OrderID
        {
            get { return _orderID++; }
        }
    }

}