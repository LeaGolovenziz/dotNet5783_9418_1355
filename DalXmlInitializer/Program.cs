using DO;
using System.Xml.Linq;

namespace Dal
{
    public class Program
    {
        static void Main()
        {
            List<Product?> products = new List<Product?>();
            List<Order?> orders = new List<Order?>();
            List<OrderItem?> orderItems = new List<OrderItem?>();
            List<User?> users = new List<User?>();


            Random rand = new Random();

            #region Data arreys

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

            string[] Names = { "Sara", "Rebeka", "Rachel", "Leah", "Naomi", "Adina" };

            string[] cities = { "Jerusalem", "Ramat Gan", "Bnei Brak", "Beit Shemesh", "Ashdod" };

            string[] streets = { "Ben Guryon", "Habrosh", "Hazait", "Vaitzman", "Begin" };

            #endregion

            try
            {
                #region Create products

                for (int i = 0; i < 50; i++)
                {
                    // create new product
                    DO.Product product = new DO.Product();

                    // draws an id while there's alredy a product in the list with the same id
                    do
                    {
                        product.ID = rand.Next(100000, 999999);
                    }
                    while (products.Exists(x => x!.Value.ID == product.ID));

                    // select the name of the product from the names array
                    product.Name = productsNamesArray[i];
                    // draw the price of the product
                    product.Price = (double)rand.Next(50, 200);
                    // the stock is 0 the  first time
                    product.InStock = i;
                    //draw the category of the froduct
                    product.Category = (DO.Enums.Category)(i % 5);
                    //
                    product.Image = "PL\\images\\" + product.Name + ".jpg";

                    // add the product to the list
                    products.Add(product);


                }

                #endregion

                #region create users

                User user=new User();

                for (int i = 0; i < 4; i++)
                {
                    // draws an id while there's alredy a product in the list with the same id
                    do
                    {
                        user.ID = 100000000 + i;
                    }
                    while (users.Exists(x => x.Value.ID == user.ID));

                    user.Name = Names[i];

                    user.Password = (111110 + i).ToString();
                    user.IsManeger = false;

                    users.Add(user);

                }

                #region Create maneger

                // draws an id while there's alredy a product in the list with the same id
                do
                {
                    user.ID = 100000004;
                }
                while (users.Exists(x => x.Value.ID == user.ID));

                user.Name = Names[5];

                user.Password = (100004).ToString();

                user.IsManeger = true;

                users.Add(user);
                #endregion

                #endregion

                #region Create Orders

                for (int i = 0; i < 20; i++)
                {
                    // create new order
                    DO.Order order = new DO.Order();

                    // gets the next available id
                    order.ID = config.OrderID;

                    User? customer = users.ElementAt(rand.Next(0, 3));

                    order.CustomerID = (int)customer?.ID!;

                    order.CustomerName = customer?.Name;

                    order.CustomerEmail = customer?.Name + "@gmail.com";

                    // draw a city and a street fron the cities and streeats arrays
                    order.CustomerAdress = streets[rand.Next(0, 4)] + " " + rand.Next(1, 100) + " " + cities[rand.Next(0, 4)];
                    // draw a date in the rang between last year and two months ago
                    order.OrderDate = DateTime.Now.Add(new TimeSpan(rand.Next(-360, 0), 0, 0, 0));

                    // about 80% of the orders have a ship date
                    if (i < 16)
                        // draw a date in the range between the order date and 7 days after
                        order.ShipDate = (order.OrderDate ?? throw new DO.Nullvalue()).Add(new TimeSpan(rand.Next(1, 7), 0, 0, 0));
                    else
                        order.ShipDate = null;

                    // about 60% of the shipped orders have a delivery date
                    if (i < 10)
                        // draw a date in the range between the ship date and 2 days after
                        order.DeliveryDate = (order.ShipDate ?? throw new DO.Nullvalue()).Add(new TimeSpan(rand.Next(1, 2), 0, 0, 0));
                    else
                        order.DeliveryDate = null;

                    orders.Add(order);

                }

                #endregion

                #region Create orders items

                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        DO.OrderItem orderItem = new DO.OrderItem();

                        // gets the next available id
                        orderItem.OrderItemID = config.ItemOrderID;
                        DO.Product p = ((DO.Product)(products[i + j])!);
                        orderItem.ID = p.ID;
                        orderItem.Price = p.Price;
                        orderItem.OrderID = 100000 + i;
                        orderItem.ProductAmount = rand.Next(1, 10);

                        orderItems.Add(orderItem);
                    }
                }

                #endregion

                #region Save lists to xml
                XElement initialize = new XElement("products",
                    from product in products
                    select new XElement
                    ("Product",
                new XElement("ID", product?.ID),
                new XElement("Name", product?.Name),
                new XElement("Category", product?.Category),
                new XElement("Price", product?.Price),
                new XElement("InStock", product?.InStock),
                new XElement("Image", product?.Image)));

                initialize.Save(@"..\..\..\..\xml\Product.xml");


                XmlTools.SaveListToXMLSerializer(orders, @"..\..\..\..\xml\Order.xml");
                XmlTools.SaveListToXMLSerializer(orderItems, @"..\..\..\..\xml\OrderItem.xml");
                XmlTools.SaveListToXMLSerializer(users, @"..\..\..\..\xml\Users.xml");


                #endregion

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


        }
        public static class config
        {
            internal static int itemOrderID = 100000;
            internal static int orderID = 100000;
            public static int ItemOrderID
            {
                get { return itemOrderID++; }
            }

            public static int OrderID
            {
                get { return orderID++; }
            }
        }
    }

}