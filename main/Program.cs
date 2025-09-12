using System;
using System.IO;
using System.Text;
using System.Numerics;

class Program
{


    static void Main()
    {
        Console.WriteLine("Name: ");

        string filePath = "Tekstas.txt";

        //string filePath = Console.ReadLine();

        string text = File.ReadAllText(filePath);
        Console.WriteLine("File contents:");
        Console.WriteLine(text);

        byte[] bytes = Encoding.UTF8.GetBytes(text);

        string bits = string.Join(" ", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        Console.WriteLine(bits); // Output: 01001000 01100101 01101100 01101100 01101111

        BigInteger number = new BigInteger(bytes);
        Console.WriteLine("Number representation:");
        Console.WriteLine(number);
    }
}