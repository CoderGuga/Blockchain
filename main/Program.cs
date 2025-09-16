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
        Console.WriteLine("Input:");
        Console.WriteLine(text);

        byte[] bytes = Encoding.UTF8.GetBytes(text);

        //string bits = string.Join(" ", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        //Console.WriteLine(bits); // Output: 01001000 01100101 01101100 01101100 01101111

        while (bytes.Length < 64)
        {
            BigInteger number = new BigInteger(bytes);
            BigInteger shiftedLeft = number << 1;
            byte[] shiftedLeftBytes = shiftedLeft.ToByteArray();

            bytes = bytes.Concat(shiftedLeftBytes).ToArray();
        }

        byte[] firstBytes = bytes.Take(32).ToArray();
        bytes = bytes.Skip(32).ToArray();

        while (bytes.Length >= 32)
        {
            byte[] xorResult = XorArrays(firstBytes, bytes.Take(32).ToArray());
            bytes = bytes.Skip(32).ToArray();
            firstBytes = xorResult;
        }

        string hexString = BitConverter.ToString(firstBytes).Replace("-", "");
        Console.WriteLine("Hash:");
        Console.WriteLine(hexString + "\n");
    }


    static byte[] XorArrays(byte[] a, byte[] b)
    {
        int length = Math.Min(a.Length, b.Length);
        byte[] result = new byte[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = (byte)(a[i] ^ b[i]);
        }
        return result;
    }
}