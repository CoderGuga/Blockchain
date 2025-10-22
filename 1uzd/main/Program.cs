using System;
using System.IO;
using System.Text;
using System.Numerics;

class Program
{
    static void Main()
    {
        string input = "";
        while (input != "1" && input != "2")
        {
            Console.WriteLine("1 for input, 2 for full test");
            input = Console.ReadLine();
            if (input == "1")
                InputText();
            if (input == "2")
                FullTest();
        }
    }

    static void FullTest()
    {
        FullAvalancheTest();
        FullPairTest();
        FullEfficiencyTest();
    }

    static void FullAvalancheTest()
    {
        Tyrimas.AvalancheEffect(100000, 100, "gab");
        Tyrimas.AvalancheEffect(100000, 100, "MD5");
        Tyrimas.AvalancheEffect(100000, 100, "SHA1");
        Tyrimas.AvalancheEffect(100000, 100, "SHA256");
        Tyrimas.AvalancheEffect(100000, 100, "titoAI");
        Tyrimas.AvalancheEffect(100000, 100, "tito");
    }

    static void FullEfficiencyTest()
    {
        Tyrimas.ReadKonstitucija("gab");
        Tyrimas.ReadKonstitucija("MD5");
        Tyrimas.ReadKonstitucija("SHA1");
        Tyrimas.ReadKonstitucija("SHA256");
        Tyrimas.ReadKonstitucija("titoAI");
        Tyrimas.ReadKonstitucija("tito");
    }

    static void FullPairTest()
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

        Tyrimas.RunPairCheck(100000, 10, "titoAI");
        Tyrimas.RunPairCheck(100000, 100, "titoAI");
        Tyrimas.RunPairCheck(100000, 500, "titoAI");
        Tyrimas.RunPairCheck(100000, 1000, "titoAI");

        Tyrimas.RunPairCheck(100000, 10, "tito");
        Tyrimas.RunPairCheck(100000, 100, "tito");
        Tyrimas.RunPairCheck(100000, 500, "tito");
        Tyrimas.RunPairCheck(100000, 1000, "tito");
    }

    static void InputText()
    {
        Console.WriteLine("1 - Skaityti is failo, 2 - Skaityti komandine eilute");
        string? input = Console.ReadLine();

        string? text = "";
        while (input != "1" && input != "2")
        {
            Console.WriteLine("Iveskite 1 arba 2");
            input = Console.ReadLine();
        }

        if (input == "1")
        {
            Console.WriteLine("Iveskite failo pavadinima");
            string? filePath = Console.ReadLine();
            text = File.ReadAllText(filePath);
        }
        else if (input == "2")
        {
            Console.WriteLine("Iveskite norima teksta");
            text = Console.ReadLine();
        }

        Console.WriteLine("1 - gab hash, 2 - tito hash, 3 - AI hash");
        string? type = Console.ReadLine();
        while (type != "1" && type != "2" && type != "3")
        {
            Console.WriteLine("Iveskite 1, 2 arba 3");
            type = Console.ReadLine();
        }
        string answer = "";
        if (type == "1")
            answer = Hashing.Hash(text);
        else if (type == "2")
            answer = Tito.Mixing(text);
        else if (type == "3")
            answer = StormHash.ComputeHash(text);

        Console.WriteLine($"Hash: \n{answer}");
        
    }
}