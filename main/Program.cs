using System;
using System.IO;
using System.Text;
using System.Numerics;

class Program
{
    static void Main()
    {
        Tyrimas.RunPairCheck(100000, 10, "gab");
        Tyrimas.RunPairCheck(100000, 100, "gab");
        Tyrimas.RunPairCheck(100000, 500, "gab");
        Tyrimas.RunPairCheck(100000, 1000, "gab");

        Tyrimas.RunPairCheck(100000, 10, "MD5");
        Tyrimas.RunPairCheck(100000, 100, "MD5");
        Tyrimas.RunPairCheck(100000, 500, "MD5");
        Tyrimas.RunPairCheck(100000, 1000, "MD5");

        Tyrimas.RunPairCheck(100000, 10, "SHA1");
        Tyrimas.RunPairCheck(100000, 100, "SHA1");
        Tyrimas.RunPairCheck(100000, 500, "SHA1");
        Tyrimas.RunPairCheck(100000, 1000, "SHA1");

        Tyrimas.RunPairCheck(100000, 10, "SHA256");
        Tyrimas.RunPairCheck(100000, 100, "SHA256");
        Tyrimas.RunPairCheck(100000, 500, "SHA256");
        Tyrimas.RunPairCheck(100000, 1000, "SHA256");
    }

    static void MaxCheck()
    {
        Tyrimas.RunPairCheck(100000, 10, "gab");
        Tyrimas.RunPairCheck(100000, 100, "gab");
        Tyrimas.RunPairCheck(100000, 500, "gab");
        Tyrimas.RunPairCheck(100000, 1000, "gab");

        Tyrimas.RunPairCheck(100000, 10, "MD5");
        Tyrimas.RunPairCheck(100000, 100, "MD5");
        Tyrimas.RunPairCheck(100000, 500, "MD5");
        Tyrimas.RunPairCheck(100000, 1000, "MD5");

        Tyrimas.RunPairCheck(100000, 10, "SHA1");
        Tyrimas.RunPairCheck(100000, 100, "SHA1");
        Tyrimas.RunPairCheck(100000, 500, "SHA1");
        Tyrimas.RunPairCheck(100000, 1000, "SHA1");

        Tyrimas.RunPairCheck(100000, 10, "SHA256");
        Tyrimas.RunPairCheck(100000, 100, "SHA256");
        Tyrimas.RunPairCheck(100000, 500, "SHA256");
        Tyrimas.RunPairCheck(100000, 1000, "SHA256");
    }

    static void InputText()
    {
        Console.WriteLine("Skaityti is failo: 1\nSkaityti komandine eilute: 2");
        string? input = Console.ReadLine();

        string text;
        while (input != "1" && input != "2")
        {
            Console.WriteLine("Iveskite 1 arba 2");
            input = Console.ReadLine();
        }

        if (input == "1")
        {
            Console.WriteLine("Iveskite failo pavadinima");
            string filePath = Console.ReadLine();
            text = File.ReadAllText(filePath);
        }
        else
        {
            Console.WriteLine("Iveskite norima teksta");
            text = Console.ReadLine();
        }

        Hashing.Hash(text);
    }
}