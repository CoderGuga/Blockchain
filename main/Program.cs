using System;
using System.IO;
using System.Text;
using System.Numerics;

class Program
{


    static void Main()
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


        byte[] hash = GetHashString(text, 32);
        BigInteger bigInteger = new BigInteger(hash);
        BigInteger mult = MultNumbers(bigInteger);
        //Console.WriteLine($"big int: {bigInteger} mult: {mult}");
        hash = GetHashString(mult.ToString(), 32);
        //int intValue = BitConverter.ToInt32(firstHash, 0);
        //byte[] newHash = ShuffleBytes(firstHash, intValue);
        //string hexString = BitConverter.ToString(newHash).Replace("-", "");

        string hexString = BitConverter.ToString(hash).Replace("-", "");

        Console.WriteLine("Hash:");
        Console.WriteLine(hexString + "\n");
        //Console.WriteLine("int value:" + intValue);
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

    static byte[] ShiftBytes(byte[] bytes, int shiftN)
    {
        BigInteger number = new BigInteger(bytes);
        BigInteger shiftedLeft = number << shiftN;
        byte[] shiftedLeftBytes = shiftedLeft.ToByteArray();

        // Prepend shiftedLeftBytes to bytes
        byte[] result = shiftedLeftBytes.Concat(bytes).ToArray();
        return result;
    }

    static void WriteBits(byte[] bytes)
    {
        string bits = string.Join(" ", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        Console.WriteLine(bits);
    }

    static byte[] GetHashString(string text, int lenght)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);

        //WriteBits(bytes.Take(1).ToArray());
        //WriteBits(ShiftBytes(bytes.Take(1).ToArray(), 1));

        while (bytes.Length < lenght * 2)
        {
            byte[] shiftedLeftBytes = ShiftBytes(bytes, 1);

            bytes = bytes.Concat(shiftedLeftBytes).ToArray();
        }

        byte[] firstBytes = bytes.Take(lenght).ToArray();
        bytes = bytes.Skip(lenght).ToArray();

        while (bytes.Length > 0)
        {
            if (bytes.Length < lenght)
            {
                byte[] padded = new byte[lenght];
                Array.Copy(bytes, padded, bytes.Length);
                bytes = padded;
            }
            //firstBytes = ShiftBytes(firstBytes, 1);
            byte[] xorResult = XorArrays(firstBytes, bytes.Take(lenght).ToArray());
            bytes = bytes.Skip(lenght).ToArray();
            firstBytes = xorResult;
        }

        return firstBytes;

    }

    static BigInteger MultNumbers(BigInteger bigInteger)
    {
        BigInteger res = 1;
        byte[] bytes = bigInteger.ToByteArray();
        foreach (byte num in bytes)
        {
            int mult = num == 0 ? 1 : num;
            res *= mult;
        }
        return res;
    }

    static byte[] ShuffleBytes(byte[] array, int seed)
    {
        Random rng = new Random(seed);
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            byte temp = array[k];
            array[k] = array[n];
            array[n] = temp;
        }

        return array;
    }
}