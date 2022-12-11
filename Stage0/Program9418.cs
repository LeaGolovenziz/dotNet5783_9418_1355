// See https://aka.ms/new-console-template for more information

namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome9418();
            Welcome1355();
            Console.ReadKey();
        }

        static partial void Welcome1355();

        private static void Welcome9418()
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();
            Console.WriteLine(name + ", welcome to my first console application");
        }
    }
}