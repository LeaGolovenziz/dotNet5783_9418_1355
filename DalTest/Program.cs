using Dal;
using DO;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static DO.Enums;
using System.Xml.Linq;

namespace DalTest
{
    public class Program
    {
        private DalOrder dalOrder = new DalOrder();
        private DalProduct dalProduct = new DalProduct();
        private DalOrderItem dalOrderItem = new DalOrderItem();

        static void Main()
        {
            Product InputProductDetails()
            {
                Console.WriteLine("Enter product's id, name, price, category and amount in stock");

                int id = int.Parse(Console.ReadLine());
                string name = Console.ReadLine();
                double price = double.Parse(Console.ReadLine());
                string category1 = Console.ReadLine();
                Enums.Category category = (Enums.Category)Convert.ToInt32(category1);
                int inStock = int.Parse(Console.ReadLine());

                Product product = new Product();

                product.ID = id;
                product.Name = name;
                product.Price = price;
                product.Category = category;
                product.InStock = inStock;

                return product;
            }
            void InputIDAndprintProduct()
            {
                Product product;

                Console.WriteLine("Enter product's id");
                int id = int.Parse(Console.ReadLine());

                product = DalProduct.Get(id);
                product.ToString();
            }

            Console.WriteLine("1. check order\n2. check product\n3. check order's item check\n0. to exit\n");
            int firstChoice = int.Parse(Console.ReadLine());
            do
            {
                char secondChoise;
                switch (firstChoice)
                {
                    case 0:
                        Console.WriteLine("bye");
                        break;
                    case 1:
                        Console.WriteLine("a. add order\nb. update order\nc. get order\nd. get all orders\ne. delete all orders");
                        secondChoise = char.Parse(Console.ReadLine());
                        switch (secondChoise)
                        {
                            case 'a':
                                break;

                            default: break;
                        }
                        break;
                    case 2:
                        Console.WriteLine("a. add product\nb. update product\nc. get product\nd. get all products\ne. delete product");
                        secondChoise = char.Parse(Console.ReadLine());
                        Console.WriteLine("Enter product's id, name, price, category and amount in stock");
                        Product product;
                        switch (secondChoise)
                        {
                            case 'a':
                                product = InputProductDetails();
                                Console.WriteLine(DalProduct.Add(product));
                                break;

                            case 'b':
                                InputIDAndprintProduct();
                                product = InputProductDetails();
                                DalProduct.Update(product);
                                break;

                            case 'c':
                                InputIDAndprintProduct();
                                break;

                            case 'd':
                                break;

                            case 'e':
                                Console.WriteLine("Enter product's id");
                                int id = int.Parse(Console.ReadLine());
                                DalProduct.Delete(id);

                                break;
                        }
                        break;

                    case 3:
                        break;

                }
            }
            while (firstChoice != 0);

        }
    }
}