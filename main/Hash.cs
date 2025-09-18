using System;
using System.IO;
using System.Text;
using System.Numerics;

public class Hashing
{
    public static void Hash(string text)
    {
        byte[] hash = GetHashString(text, 32);
        BigInteger bigInteger = new BigInteger(hash);
        BigInteger mult = MultNumbers(bigInteger);
        hash = GetHashString(mult.ToString(), 32);

        string hexString = BitConverter.ToString(hash).Replace("-", "");

        Console.WriteLine("Hash:");
        Console.WriteLine(hexString + "\n");
    }

    public static string HashString(string text)
    {
        byte[] hash = GetHashString(text, 32);
        BigInteger bigInteger = new BigInteger(hash);
        BigInteger mult = MultNumbers(bigInteger);
        hash = GetHashString(mult.ToString(), 32);

        string hexString = BitConverter.ToString(hash).Replace("-", "");

        return hexString;
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
}