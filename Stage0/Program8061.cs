
using System;
namespace Targil0 { 


partial class Program
{
    private static void Main(string[] args)
    {
        Welcome8061();
        Welcome2833();
        Console.ReadKey();

    }
    static partial void Welcome2833();
    private static void Welcome8061()
    {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();

        Console.WriteLine(name + ", welcome to my first console application");
    }
}
}