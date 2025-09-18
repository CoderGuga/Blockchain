using System;
using System.IO;
using System.Text;
using System.Numerics;

class Program
{
    static void Main()
    {
        Tyrimas.AvalancheEffect(100000, 20);
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