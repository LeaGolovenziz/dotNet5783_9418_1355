using Dal;
using DO;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace DalTest
{
    public class Program
    {
        private DalOrder dalOrder = new DalOrder();
        private DalProduct dalProduct  = new DalProduct();
        private DalOrderItem dalOrderItem = new DalOrderItem();
        static void Main()
        {

            Console.WriteLine("1. check order\n2. check product\n3. check order's item check\n0. to exit\n");
            int firstChoice = int.Parse(Console.ReadLine());
            do
            {
                switch (firstChoice)
                {
                    case 0: Console.WriteLine("bye");
                        break;
                    case 1: Console.WriteLine("a. add order\nb. update product\nc. get order\nd. get all orders\ne. delete all orders");
                        char secondChoise = char.Parse(Console.ReadLine());
                        switch(secondChoise)
                        {
                            case 'a':                     
                                break;

                            default: break;                       
                        }
                        break;
                    case 2:
                        break;

                }
            }
            while (firstChoice != 0);
                             
        }
    }
}