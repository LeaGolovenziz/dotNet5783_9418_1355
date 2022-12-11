using DO;

namespace Dal;

internal static class DataSource
{
    /// <summary>
    /// unique random varient to draw numbers
    /// </summary>
    static readonly Random _rand = new Random();

    internal static List<Order?> _lstOreders = new List<Order?>();
    internal static List<OrderItem?> _lstOrderItems = new List<OrderItem?>();
    internal static List<Product?> _lstPruducts = new List<Product?>();

    static DataSource()
    {
        s_Initialize();
    }
    private static void s_Initialize()
    {
        // array of product's possible names
        string[] productsNamesArray = { "Cypress tree", "Rose", "Black coral snake plant", "Watering can", "Fast acting iron",
                                        "Apple tree", "Tulip", "Whale fin snake plant", "Soil soker hose", "Fast a thing gypsum",
                                        "Peach tree", "Orchid", "Calathea ornata", "Pruner", "Fast acting lime",
                                        "Cherry tree", "Iris", "Moonshine snake plant", "Bypass loppers", "Compost starter",
                                        "Berry bushes", "Lily", "Calathea makoyana", "Grip trowel", "Bone mael",
                                        "Beech tree", "Anemone", "Calathea orbifolia", "Saw", "Organic potting mix",
                                        "Maple tree", "Daisy", "Majesty palm", "Hedge shear", "Organic cactus mix",
                                        "Redbud tree", "Aster", "Parlor palm", "Garden gloves", "Potting soil",
                                        "Cotton tree", "Sunflower", "Bamboo palm", "Tree staking kit", "Moss max",
                                        "Treaty tree", "Lavender", "Peach lily", "Pump and spray applicator", "Iron tone" };

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
                product.ID = _rand.Next(100000, 999999);
            }
            while (_lstPruducts.Exists(x => x.Value.ID == product.ID));

            // select the name of the product from the names array
            product.Name = productsNamesArray[i];
            // draw the price of the product
            product.Price = (double)_rand.Next(50, 200);
            // the stock is 0 the  first time
            product.InStock = i;
            //draw the category of the froduct
            product.Category = (Enums.Category)(i % 5);

            // add the product to the list
            _lstPruducts.Add(product);
        }

        for (int i = 0; i < 20; i++)
        {
            // create new order
            Order order = new Order();

            // gets the next available id
            order.ID = config.OrderID;
            // draw a name and last name from the names and last names arrays
            string custumerFirstName = firstNames[_rand.Next(0, 4)];
            string custumerLastName = lastNames[_rand.Next(0, 4)];

            order.CustomerName = custumerFirstName + " " + custumerLastName;
            order.CustomerEmail = custumerFirstName + custumerLastName + "@gmail.com";
            // draw a city and a street fron the cities and streeats arrays
            order.CustomerAdress = streets[_rand.Next(0, 4)] + " " + _rand.Next(1, 100) + " " + cities[_rand.Next(0, 4)];
            // draw a date in the rang between last year and two months ago
            order.OrderDate = DateTime.Now.Add(new TimeSpan(_rand.Next(-360, 0), 0, 0, 0));

            // about 80% of the orders have a ship date
            if (i < 16)
                // draw a date in the range between the order date and 7 days after
                order.ShipDate = order.OrderDate?.Add(new TimeSpan(_rand.Next(1, 7), 0, 0, 0));
            else
                order.ShipDate = null;

            // about 60% of the shipped orders have a delivery date
            if (i < 10)
                // draw a date in the range between the ship date and 2 days after
                order.DeliveryDate = order.ShipDate?.Add(new TimeSpan(_rand.Next(1, 2), 0, 0, 0));
            else
                order.DeliveryDate = null;

            _lstOreders.Add(order);
        }
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                OrderItem orderItem = new OrderItem();

                orderItem.OrderItemID = config.ItemOrderID;
                Product p = (Product)_lstPruducts[_rand.Next(0, 9)]!;
                orderItem.ProductID = p.ID;
                orderItem.ProductPrice = p.Price;
                orderItem.OrderID = 100000 + i;
                orderItem.ProductAmount = _rand.Next(1, 10);

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

        public static int ItemOrderID
        {
            get { return _itemOrderID++; }
        }

        public static int OrderID
        {
            get { return _orderID++; }
        }
    }

}
