using System.Diagnostics;

public class Tyrimas
{
    public static void ReadKonstitucija()
    {
        int lineCount = 1;

        string konstitucija = File.ReadAllText("tyrimas/konstitucija.txt");
        string[] lines = konstitucija.Split('\n');

        string result = "";

        while (lineCount < lines.Count())
        {
            string text = string.Join("\n", lines.Take(lineCount));
            //Console.WriteLine(text+"\n");
            Stopwatch sw = Stopwatch.StartNew();
            Hashing.Hash(text);
            sw.Stop();
            result += $"Line Count {lineCount.ToString("D3")} Time {sw.ElapsedTicks.ToString("D6")} ticks\n";
            lineCount *= 2;
        }

        File.WriteAllText("Tyrimas/timeResults.txt", result);
    }

    public static int RunPairCheck(int count, int lenght)
    {
        int colCount = 0;

        for (int i = 0; i < count; i++)
        {
            if (CheckHashPairs(lenght))
                colCount++;
        }

        return colCount;
    }

    public static bool CheckHashPairs(int lenght)
    {
        string hash1 = Hashing.HashString(GenerateRandomString(lenght));
        string hash2 = Hashing.HashString(GenerateRandomString(lenght));

        //File.AppendAllText("Tyrimas/collisionResults.txt", $"{hash1} - {hash2}\n");
        return hash1 == hash2 ? true : false;
    }

    static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }

    public static void AvalancheEffect(int pairCount, int stringLength)
    {
        int minBitDiff = int.MaxValue, maxBitDiff = int.MinValue, totalBitDiff = 0;
        int minHexDiff = int.MaxValue, maxHexDiff = int.MinValue, totalHexDiff = 0;

        for (int i = 0; i < pairCount; i++)
        {
            string baseStr = GenerateRandomString(stringLength);
            char[] arr = baseStr.ToCharArray();
            int pos = new Random().Next(stringLength);
            arr[pos] = arr[pos] == 'A' ? 'B' : 'A'; // Change one symbol
            string modStr = new string(arr);

            string hash1 = Hashing.HashString(baseStr);
            string hash2 = Hashing.HashString(modStr);

            byte[] hash1B = hash1.Select(c => Convert.ToByte(c)).ToArray();
            byte[] hash2B = hash2.Select(c => Convert.ToByte(c)).ToArray();

            // Bit difference
            int bitDiff = 0;
            for (int j = 0; j < Math.Min(hash1B.Length, hash2B.Length); j++)
            {
                bitDiff += Convert.ToString(hash1B[j] ^ hash2B[j], 2).Count(b => b == '1');
            }
            minBitDiff = Math.Min(minBitDiff, bitDiff);
            maxBitDiff = Math.Max(maxBitDiff, bitDiff);
            totalBitDiff += bitDiff;

            // Hex difference
            int hexDiff = hash1.Zip(hash2, (a, b) => a == b ? 0 : 1).Sum();
            minHexDiff = Math.Min(minHexDiff, hexDiff);
            maxHexDiff = Math.Max(maxHexDiff, hexDiff);
            totalHexDiff += hexDiff;
        }

        Console.WriteLine($"Bit difference: min={minBitDiff}, max={maxBitDiff}, avg={(double)totalBitDiff / pairCount}");
        Console.WriteLine($"Hex difference: min={minHexDiff}, max={maxHexDiff}, avg={(double)totalHexDiff / pairCount}");
    }

    static byte[] HexStringToBytes(string hex)
    {
        int len = hex.Length;
        byte[] bytes = new byte[len / 2];
        for (int i = 0; i < len; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }
}