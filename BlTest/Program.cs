using BlApi;
using Bllmplementation;
using BO;
using Order = BO.Order;
using OrderItem = BO.OrderItem;
using Product = BO.Product;

namespace BlTest
{
    internal class Program
    {
        private static IBl _iBl = new Bl();

        static void Main()
        {
            // gets customer details for cart
            Cart getCartDetails(Cart cart)
            {
                Console.WriteLine("Enter customer's name:");
                string name = Console.ReadLine()!;
                cart.CustomerName = name;

                Console.WriteLine("Enter customer's email:");
                string email = Console.ReadLine()!;
                cart.CustomerEmail = email;

                Console.WriteLine("Enter customer's address:");
                string address = Console.ReadLine()!;
                cart.CustomerAddress = address;

                return cart;    
            }

            // inputs an ID of order
            int inputOrderId()
            {
                int id;

                // input the ID of the order
                Console.WriteLine("Enter order's ID:");
                // while the ID is unvalid - inform and input again
                while (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine("bad input, enter again");
                }
                return id;
            }

            void inputSecondCoice(out char secondChoice)
            {
                // while the second choice is unvalid - inpirm and input a new one
                while (!char.TryParse(Console.ReadLine(), out secondChoice))
                {
                    Console.WriteLine("bad input, enter again");
                }
            }

            // inputs an ID of order
            int inputProductId()
            {
                int id;
                // input the ID of the product
                Console.WriteLine("Enter product's ID:");
                // while the ID is unvalid - inform and input again
                while (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine("bad input, enter again");
                }
                return id;
            }

            // gets product that have an ID, inputs its other details and returns it
            Product GetProductDetails( Product product)
            {
                product.ID = inputOrderId();

                // input the name of the product
                Console.WriteLine("Enter product's name:");
                string name = Console.ReadLine()!;
                product.Name = name;

                // input the price of the product
                Console.WriteLine("Enter product's price:");
                double price;
                // while the price is unvalid - inform and input again
                while (!double.TryParse(Console.ReadLine(), out price))
                {
                    Console.WriteLine("bad input, enter again");
                }
                product.Price = price;

                // input the category of the product
                Console.WriteLine("Enter product's category:");
                BO.Enums.Category category;
                // while the category is unvalid - inform and input again
                while (!BO.Enums.Category.TryParse(Console.ReadLine(), out category))
                {
                    Console.WriteLine("bad input, enter again");
                }
                product.Category = category;

                // input the amount in stock of the product
                Console.WriteLine("Enter product's amount in stock:");
                int inStock;
                // while the category is unvalid - inform and input again
                while (!int.TryParse(Console.ReadLine(), out inStock))
                {
                    Console.WriteLine("bad input, enter again");
                }
                product.InStock = inStock;

                return product; 
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

            int firstChoice = 0;
            char secondChoice;
            Order order = new Order();
            Product product = new Product();
            Cart cart = new Cart();

            cart.OrderItems = new List<OrderItem>(); // initializing the cart's items list

            ProductItem productItem;

            do
            {
                Console.WriteLine("1. check order\n2. check product\n3. check cart \n0. to exit\n");
                string tempFirstChoice = Console.ReadLine();
                int.TryParse(tempFirstChoice, out firstChoice);

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
                            inputSecondCoice(out secondChoice);

                            try
                            {
                                switch (secondChoice)
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
                                        order = _iBl.Order.GetOrderDetails(inputOrderId());

                                        Console.WriteLine(order);
                                        break;

                                    // update order's delivery
                                    case 'd':
                                        order = _iBl.Order.DeliverOrder(inputOrderId());

                                        Console.WriteLine(order);

                                        break;

                                    // update order's shipping
                                    case 's':
                                        order = _iBl.Order.ShipOrder(inputOrderId());

                                        Console.WriteLine(order);

                                        break;

                                    // track order
                                    case 't':
                                        OrderTracking orderT = _iBl.Order.TrackOrder(inputOrderId());

                                        Console.WriteLine(orderT);

                                        break;

                                    // update order's details
                                    case 'u':
                                        // input details of the update
                                        int orderId = inputOrderId();
                                        int productID = inputProductId();
                                        int newAmount;
                                        Console.WriteLine("Enter new amount");
                                        while (!int.TryParse(Console.ReadLine(), out newAmount))
                                        {
                                            Console.WriteLine("bad input, enter again");
                                        }

                                        order = _iBl.Order.UpdateOrderDetails(orderId, productID, newAmount);

                                        Console.WriteLine(order);

                                        break;
                                }
                            }
                            catch (blExceptions ex)
                            {
                                catchErrors(ex);
                            }
                        }
                        while (!secondChoice.Equals('e'));


                        break;
                    // product operations
                    case 2:
                        do
                        {
                            Console.WriteLine("g. get products' list\np. get product's details\nc. get product from catalog\na. add product\nd. delete product\nu. update product's details\ne. return to main menue\n");
                            inputSecondCoice(out secondChoice);

                            try
                            {
                                switch (secondChoice)
                                {
                                    // get and print product's list
                                    case 'g':
                                        IEnumerable<ProductForList> products = _iBl.Product.GetProductsList()!;

                                        foreach (ProductForList productForList in products)
                                        {
                                            Console.WriteLine(productForList);
                                        }

                                        break;

                                    // get and print product's details
                                    case 'p':

                                        product = _iBl.Product.GetProductDetails(inputProductId());

                                        Console.WriteLine(product);

                                        break;

                                    // get ant print catalog's product's details
                                    case 'c':
                                        productItem = _iBl.Product.GetProductFromCatalog(inputProductId(), cart);

                                        Console.WriteLine(productItem);

                                        break;

                                    // add a product
                                    case 'a':
                                        product=GetProductDetails(product);

                                        _iBl.Product.AddProduct(product);

                                        break;

                                    // delete a product
                                    case 'd':
                                        _iBl.Product.DeleteProduct(inputProductId());

                                        break;

                                    // update product's details
                                    case 'u':
                                        product=GetProductDetails(product);

                                        _iBl.Product.UpdateProduct(product);

                                        break;

                                    // return to main menue
                                    case 'e':

                                        break;


                                    default: break;
                                }
                            }
                            catch (blExceptions ex)
                            {
                                catchErrors(ex);
                            }
                        }
                        while (secondChoice != 'e');
                        break;

                    // cart's operations
                    case 3:
                        do
                        {
                            Console.WriteLine("a. add produc to cart\nu. update product's amount\np. place order\ne. return to main menue");
                            inputSecondCoice(out secondChoice);

                            try
                            {
                                switch (secondChoice)
                                {
                                    // add a product to cart
                                    case 'a':
                                        int productID = inputProductId();
                                        product = _iBl.Product.GetProductDetails(productID);

                                        _iBl.Cart.AddProductToCart(cart, productID);

                                        break;

                                    // update product in cart
                                    case 'u':
                                        int productId = inputProductId();
                                        product = _iBl.Product.GetProductDetails(inputProductId());

                                        int amount;
                                        Console.WriteLine("Enter product's new amount");
                                        while (!int.TryParse(Console.ReadLine(), out amount))
                                        {
                                            Console.WriteLine("baf input, enter again");
                                        }

                                        _iBl.Cart.UpdateProductAmountInCart(cart, productId, amount);

                                        break;

                                    // make an order from the cart
                                    case 'p':
                                        cart= getCartDetails(cart);

                                        _iBl.Cart.PlaceOrder(cart);

                                        break;

                                    // return to main menue
                                    case 'e':

                                        break;

                                    default: break;
                                }
                            }
                            catch (blExceptions ex)
                            {
                                catchErrors(ex);
                            }
                        }
                        while (secondChoice != 'e');
                        break;
                }

            }
            while (firstChoice != 0);
        }
    }
}

/*
 example:
1. check order
2. check product
3. check cart
0. to exit

1
g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

g

        Order ID - 100000:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 3
        Total price: 144


        Order ID - 100001:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 1
        Total price: 169


        Order ID - 100002:
        Custumer's name: Sara Goldenkoif
        Order's status: Confirmed
        Amount of items: 7
        Total price: 113


        Order ID - 100003:
        Custumer's name: Leah Goldenkoif
        Order's status: Confirmed
        Amount of items: 5
        Total price: 169


        Order ID - 100004:
        Custumer's name: Leah Goldenkoif
        Order's status: Confirmed
        Amount of items: 7
        Total price: 183


        Order ID - 100005:
        Custumer's name: Rebeka Goldenkoif
        Order's status: Confirmed
        Amount of items: 9
        Total price: 144


        Order ID - 100006:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 4
        Total price: 154


        Order ID - 100007:
        Custumer's name: Sara Israel
        Order's status: Confirmed
        Amount of items: 6
        Total price: 171


        Order ID - 100008:
        Custumer's name: Leah Cohen
        Order's status: Confirmed
        Amount of items: 4
        Total price: 83


        Order ID - 100009:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 2
        Total price: 183


        Order ID - 100010:
        Custumer's name: Leah Levi
        Order's status: Confirmed
        Amount of items: 7
        Total price: 113


        Order ID - 100011:
        Custumer's name: Sara Goldenkoif
        Order's status: Confirmed
        Amount of items: 7
        Total price: 142


        Order ID - 100012:
        Custumer's name: Leah Cohen
        Order's status: Confirmed
        Amount of items: 3
        Total price: 144


        Order ID - 100013:
        Custumer's name: Rebeka Levi
        Order's status: Confirmed
        Amount of items: 3
        Total price: 83


        Order ID - 100014:
        Custumer's name: Rachel Goldenkoif
        Order's status: Confirmed
        Amount of items: 6
        Total price: 183


        Order ID - 100015:
        Custumer's name: Sara Goldenkoif
        Order's status: Confirmed
        Amount of items: 1
        Total price: 171


        Order ID - 100016:
        Custumer's name: Sara Israel
        Order's status: Confirmed
        Amount of items: 8
        Total price: 171


        Order ID - 100017:
        Custumer's name: Leah Goldenkoif
        Order's status: Confirmed
        Amount of items: 4
        Total price: 83


        Order ID - 100018:
        Custumer's name: Leah Cohen
        Order's status: Confirmed
        Amount of items: 5
        Total price: 169


        Order ID - 100019:
        Custumer's name: Leah Levi
        Order's status: Confirmed
        Amount of items: 3
        Total price: 51

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

o
Enter order's id
100000

        Order's ID - 100000:

        Custumer's name: Leah Israel
                   Email: LeahIsrael@gmail.com
                   adress: Hazait 56 Ramat Gan

        Order's status: Delivered
               order date - 10/05/2022 13:30:00
               ship date - 14/05/2022 13:30:00
               delivery date - 15/05/2022 13:30:00

        List of items:

        Order ID - 100000:
        Product's ID: 402393
                  name: product9
                  price: 144
                  amount: 3
        Total price: 432


        Order ID - 100000:
        Product's ID: 105284
                  name: product3
                  price: 83
                  amount: 6
        Total price: 498


        Total price: 930

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

d
Enter order's id
100000

AlreadyDelivered
The order has already been delivered

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

s
Enter order's id
100000

AlreadyShipped
The order has already been shipped

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

t
Enter order's id
100000

        Order tracking ID - 100000:
        Order's status: Delivered
        tracking:
              (10/05/2022 13:30:00, Confirmed)
(14/05/2022 13:30:00, Sent)
(15/05/2022 13:30:00, Delivered)

        end of racking

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

u
Enter order's id
100000
Enter product's id
192
Enter new amount
3

AlreadyShipped
The order has already been shipped

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

e
1. check order
2. check product
3. check cart
0. to exit

2
g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

g

        Product ID - 118611:
        Product's name: product1
                  price: 154
                  category: category2


        Product ID - 659187:
        Product's name: product2
                  price: 169
                  category: category1


        Product ID - 105284:
        Product's name: product3
                  price: 83
                  category: category1


        Product ID - 538927:
        Product's name: product4
                  price: 171
                  category: category4


        Product ID - 823178:
        Product's name: product5
                  price: 142
                  category: category1


        Product ID - 748728:
        Product's name: product6
                  price: 183
                  category: category2


        Product ID - 514704:
        Product's name: product7
                  price: 51
                  category: category4


        Product ID - 935518:
        Product's name: product8
                  price: 113
                  category: category4


        Product ID - 402393:
        Product's name: product9
                  price: 144
                  category: category3


        Product ID - 586041:
        Product's name: product10
                  price: 149
                  category: category3

g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

a
Enter product's ID:
100000
Enter product's name:
neomi
Enter product's price:
50
Enter product's category:
category1
Enter product's amount in stock:
5
g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

g

        Product ID - 118611:
        Product's name: product1
                  price: 154
                  category: category2


        Product ID - 659187:
        Product's name: product2
                  price: 169
                  category: category1


        Product ID - 105284:
        Product's name: product3
                  price: 83
                  category: category1


        Product ID - 538927:
        Product's name: product4
                  price: 171
                  category: category4


        Product ID - 823178:
        Product's name: product5
                  price: 142
                  category: category1


        Product ID - 748728:
        Product's name: product6
                  price: 183
                  category: category2


        Product ID - 514704:
        Product's name: product7
                  price: 51
                  category: category4


        Product ID - 935518:
        Product's name: product8
                  price: 113
                  category: category4


        Product ID - 402393:
        Product's name: product9
                  price: 144
                  category: category3


        Product ID - 586041:
        Product's name: product10
                  price: 149
                  category: category3


        Product ID - 100000:
        Product's name: neomi
                  price: 50
                  category: category1

g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

p
Enter product's id
100000

        Product's ID - 100000:
                  name: neomi
                  category - category1
                  price: 50
                  amount in stock: 5

g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

c
Enter product's id
100000

        Product ID - 100000:
        Product's name: neomi
                  price: 50
                  category: category1
        Is in stock: true
        Amount of product in cart: 0

g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

g

        Product ID - 118611:
        Product's name: product1
                  price: 154
                  category: category2


        Product ID - 659187:
        Product's name: product2
                  price: 169
                  category: category1


        Product ID - 105284:
        Product's name: product3
                  price: 83
                  category: category1


        Product ID - 538927:
        Product's name: product4
                  price: 171
                  category: category4


        Product ID - 823178:
        Product's name: product5
                  price: 142
                  category: category1


        Product ID - 748728:
        Product's name: product6
                  price: 183
                  category: category2


        Product ID - 514704:
        Product's name: product7
                  price: 51
                  category: category4


        Product ID - 935518:
        Product's name: product8
                  price: 113
                  category: category4


        Product ID - 402393:
        Product's name: product9
                  price: 144
                  category: category3


        Product ID - 586041:
        Product's name: product10
                  price: 149
                  category: category3


        Product ID - 100000:
        Product's name: neomi
                  price: 50
                  category: category1

g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

39
g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

p
Enter product's id
39

DoesntExist
The requested object doesn't exist
NotFound
The requested object wasn't found

g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

d
Enter product's id
586041
g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

g

        Product ID - 118611:
        Product's name: product1
                  price: 154
                  category: category2


        Product ID - 659187:
        Product's name: product2
                  price: 169
                  category: category1


        Product ID - 105284:
        Product's name: product3
                  price: 83
                  category: category1


        Product ID - 538927:
        Product's name: product4
                  price: 171
                  category: category4


        Product ID - 823178:
        Product's name: product5
                  price: 142
                  category: category1


        Product ID - 748728:
        Product's name: product6
                  price: 183
                  category: category2


        Product ID - 514704:
        Product's name: product7
                  price: 51
                  category: category4


        Product ID - 935518:
        Product's name: product8
                  price: 113
                  category: category4


        Product ID - 402393:
        Product's name: product9
                  price: 144
                  category: category3


        Product ID - 100000:
        Product's name: neomi
                  price: 50
                  category: category1

g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

u
Enter product's ID:
100000
Enter product's name:
calanit
Enter product's price:
50
Enter product's category:
category1
Enter product's amount in stock:
5
g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

e
1. check order
2. check product
3. check cart
0. to exit

3
a. add produc to cart
u. update product's amount
p. place order
e. return to main menue
a
Enter product's id
100000
a. add produc to cart
u. update product's amount
p. place order
e. return to main menue
u
Enter product's id
100000
Enter product's new amount
3
a. add produc to cart
u. update product's amount
p. place order
e. return to main menue
p
Enter customer's name:
lea
Enter customer's email:
cohen
Enter customer's address:
jerujalem
a. add produc to cart
u. update product's amount
p. place order
e. return to main menue
e
1. check order
2. check product
3. check cart
0. to exit

1
g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

g

        Order ID - 100000:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 3
        Total price: 144


        Order ID - 100001:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 1
        Total price: 169


        Order ID - 100002:
        Custumer's name: Sara Goldenkoif
        Order's status: Confirmed
        Amount of items: 7
        Total price: 113


        Order ID - 100003:
        Custumer's name: Leah Goldenkoif
        Order's status: Confirmed
        Amount of items: 5
        Total price: 169


        Order ID - 100004:
        Custumer's name: Leah Goldenkoif
        Order's status: Confirmed
        Amount of items: 7
        Total price: 183


        Order ID - 100005:
        Custumer's name: Rebeka Goldenkoif
        Order's status: Confirmed
        Amount of items: 9
        Total price: 144


        Order ID - 100006:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 4
        Total price: 154


        Order ID - 100007:
        Custumer's name: Sara Israel
        Order's status: Confirmed
        Amount of items: 6
        Total price: 171


        Order ID - 100008:
        Custumer's name: Leah Cohen
        Order's status: Confirmed
        Amount of items: 4
        Total price: 83


        Order ID - 100009:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 2
        Total price: 183


        Order ID - 100010:
        Custumer's name: Leah Levi
        Order's status: Confirmed
        Amount of items: 7
        Total price: 113


        Order ID - 100011:
        Custumer's name: Sara Goldenkoif
        Order's status: Confirmed
        Amount of items: 7
        Total price: 142


        Order ID - 100012:
        Custumer's name: Leah Cohen
        Order's status: Confirmed
        Amount of items: 3
        Total price: 144


        Order ID - 100013:
        Custumer's name: Rebeka Levi
        Order's status: Confirmed
        Amount of items: 3
        Total price: 83


        Order ID - 100014:
        Custumer's name: Rachel Goldenkoif
        Order's status: Confirmed
        Amount of items: 6
        Total price: 183


        Order ID - 100015:
        Custumer's name: Sara Goldenkoif
        Order's status: Confirmed
        Amount of items: 1
        Total price: 171


        Order ID - 100016:
        Custumer's name: Sara Israel
        Order's status: Confirmed
        Amount of items: 8
        Total price: 171


        Order ID - 100017:
        Custumer's name: Leah Goldenkoif
        Order's status: Confirmed
        Amount of items: 4
        Total price: 83


        Order ID - 100018:
        Custumer's name: Leah Cohen
        Order's status: Confirmed
        Amount of items: 5
        Total price: 169


        Order ID - 100019:
        Custumer's name: Leah Levi
        Order's status: Confirmed
        Amount of items: 3
        Total price: 51


        Order ID - 100020:
        Custumer's name: lea
        Order's status: Confirmed
        Amount of items: 3
        Total price: 50

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

u
Enter order's id
100020
Enter product's id
100000
Enter new amount
2

        Order's ID - 100020:

        Custumer's name: lea
                   Email: cohen
                   adress: jerujalem

        Order's status: Confirmed
               order date - 28/11/2022 13:36:21
               ship date -
               delivery date -

        List of items:

        Order ID - 100020:
        Product's ID: 100000
                  name: calanit
                  price: 100
                  amount: 2
        Total price: 200


        Total price: 200

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

d
Enter order's id
100020

wasntShipped
The order wasn't shipped

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

g

        Order ID - 100000:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 3
        Total price: 144


        Order ID - 100001:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 1
        Total price: 169


        Order ID - 100002:
        Custumer's name: Sara Goldenkoif
        Order's status: Confirmed
        Amount of items: 7
        Total price: 113


        Order ID - 100003:
        Custumer's name: Leah Goldenkoif
        Order's status: Confirmed
        Amount of items: 5
        Total price: 169


        Order ID - 100004:
        Custumer's name: Leah Goldenkoif
        Order's status: Confirmed
        Amount of items: 7
        Total price: 183


        Order ID - 100005:
        Custumer's name: Rebeka Goldenkoif
        Order's status: Confirmed
        Amount of items: 9
        Total price: 144


        Order ID - 100006:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 4
        Total price: 154


        Order ID - 100007:
        Custumer's name: Sara Israel
        Order's status: Confirmed
        Amount of items: 6
        Total price: 171


        Order ID - 100008:
        Custumer's name: Leah Cohen
        Order's status: Confirmed
        Amount of items: 4
        Total price: 83


        Order ID - 100009:
        Custumer's name: Leah Israel
        Order's status: Confirmed
        Amount of items: 2
        Total price: 183


        Order ID - 100010:
        Custumer's name: Leah Levi
        Order's status: Confirmed
        Amount of items: 7
        Total price: 113


        Order ID - 100011:
        Custumer's name: Sara Goldenkoif
        Order's status: Confirmed
        Amount of items: 7
        Total price: 142


        Order ID - 100012:
        Custumer's name: Leah Cohen
        Order's status: Confirmed
        Amount of items: 3
        Total price: 144


        Order ID - 100013:
        Custumer's name: Rebeka Levi
        Order's status: Confirmed
        Amount of items: 3
        Total price: 83


        Order ID - 100014:
        Custumer's name: Rachel Goldenkoif
        Order's status: Confirmed
        Amount of items: 6
        Total price: 183


        Order ID - 100015:
        Custumer's name: Sara Goldenkoif
        Order's status: Confirmed
        Amount of items: 1
        Total price: 171


        Order ID - 100016:
        Custumer's name: Sara Israel
        Order's status: Confirmed
        Amount of items: 8
        Total price: 171


        Order ID - 100017:
        Custumer's name: Leah Goldenkoif
        Order's status: Confirmed
        Amount of items: 4
        Total price: 83


        Order ID - 100018:
        Custumer's name: Leah Cohen
        Order's status: Confirmed
        Amount of items: 5
        Total price: 169


        Order ID - 100019:
        Custumer's name: Leah Levi
        Order's status: Confirmed
        Amount of items: 3
        Total price: 51


        Order ID - 100020:
        Custumer's name: lea
        Order's status: Confirmed
        Amount of items: 2
        Total price: 100

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

u
Enter order's id
100020
Enter product's id
20
Enter new amount
2

DoesntExist
The requested object doesn't exist
NotFound
The requested object wasn't found

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

d
Enter order's id
100020

wasntShipped
The order wasn't shipped

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

s
Enter order's id
100020

        Order's ID - 100020:

        Custumer's name: lea
                   Email: cohen
                   adress: jerujalem

        Order's status: Sent
               order date - 28/11/2022 13:36:21
               ship date - 28/11/2022 13:37:53
               delivery date -

        List of items:

        Order ID - 100020:
        Product's ID: 100000
                  name: calanit
                  price: 100
                  amount: 2
        Total price: 200


        Total price: 200

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

d
Enter order's id
100020

        Order's ID - 100020:

        Custumer's name: lea
                   Email: cohen
                   adress: jerujalem

        Order's status: Delivered
               order date - 28/11/2022 13:36:21
               ship date - 28/11/2022 13:37:53
               delivery date - 28/11/2022 13:38:01

        List of items:

        Order ID - 100020:
        Product's ID: 100000
                  name: calanit
                  price: 100
                  amount: 2
        Total price: 200


        Total price: 200

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

u
Enter order's id
100020
Enter product's id
12
Enter new amount
4

AlreadyShipped
The order has already been shipped

g. get orders' list
o. get order's details
d. update order's delivery
s. update order's shipping
t. track order
u. update order's details
e. return to main menue

e
1. check order
2. check product
3. check cart
0. to exit

2
g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

d
Enter product's id
100020

DoesntExist
The requested object doesn't exist
NotFound
The requested object wasn't found

g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

d
Enter product's id
100000

ProductExistInOrder
The product exist in an order, hence can not be deleted

g. get products' list
p. get product's details
c. get product from catalog
a. add product
d. delete product
u. update product's details
e. return to main menue

e
1. check order
2. check product
3. check cart
0. to exit

0
bye
 */
