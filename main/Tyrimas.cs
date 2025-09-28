using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;

public class Tyrimas
{
    public static void ReadKonstitucija(string hashType)
    {
        int lineCount = 1;

        string konstitucija = File.ReadAllText("tyrimas/konstitucija.txt");
        string[] lines = konstitucija.Split('\n');

        string result = "";

        while (lineCount < lines.Count())
        {
            string text = string.Join("\n", lines.Take(lineCount));
            //Console.WriteLine(text+"\n");
            Stopwatch sw;
            if (hashType == "gab")
            {
                sw = Stopwatch.StartNew();
                Hashing.Hash(text);
                sw.Stop();
            }
            else if (hashType == "MD5")
            {
                sw = Stopwatch.StartNew();
                GetMd5Hash(text);
                sw.Stop();
            }
            else if (hashType == "SHA1")
            {
                sw = Stopwatch.StartNew();
                GetSha1(text);
                sw.Stop();
            }
            else if (hashType == "SHA256")
            {
                sw = Stopwatch.StartNew();
                GetSha256(text);
                sw.Stop();
            }
            else if (hashType == "titoAI")
            {
                sw = Stopwatch.StartNew();
                StormHash.ComputeHash(text);
                sw.Stop();
            }
            else if (hashType == "tito")
            {
                sw = Stopwatch.StartNew();
                Tito.Mixing(text);
                sw.Stop();
            }
            else
            {
                Console.WriteLine("Wrong hash type");
                return;
            }
            result += $"Line Count {lineCount.ToString("D3")} Time {sw.ElapsedTicks.ToString("D6")} ticks\n";
            lineCount *= 2;
        }

        File.AppendAllText("tyrimas/efficiencyResults.txt", $"{hashType}:\n{ result} \n");
    }

    public static int RunPairCheck(int count, int lenght, string hashType)
    {
        int colCount = 0;

        for (int i = 0; i < count; i++)
        {
            if (CheckHashPairs(lenght, hashType))
                colCount++;
        }


        File.AppendAllText("tyrimas/collisionResults.txt", $"{hashType}:\n String lenght: {lenght} Collision count: {colCount} \n");
        return colCount;
    }

    public static bool CheckHashPairs(int lenght, string hashType)
    {
        string hash1;
        string hash2;

        if (hashType == "gab")
        {
            hash1 = Hashing.HashString(GenerateRandomString(lenght));
            hash2 = Hashing.HashString(GenerateRandomString(lenght));
        }
        else if (hashType == "MD5")
        {
            hash1 = GetMd5Hash(GenerateRandomString(lenght));
            hash2 = GetMd5Hash(GenerateRandomString(lenght));
        }
        else if (hashType == "SHA1")
        {
            hash1 = GetSha1(GenerateRandomString(lenght));
            hash2 = GetSha1(GenerateRandomString(lenght));
        }
        else if (hashType == "SHA256")
        {
            hash1 = GetSha256(GenerateRandomString(lenght));
            hash2 = GetSha256(GenerateRandomString(lenght));
        }
        else if (hashType == "titoAI")
        {
            hash1 = StormHash.ComputeHash(GenerateRandomString(lenght));
            hash2 = StormHash.ComputeHash(GenerateRandomString(lenght));
        }
        else if (hashType == "tito")
        {
            hash1 = Tito.Mixing(GenerateRandomString(lenght));
            hash2 = Tito.Mixing(GenerateRandomString(lenght));
        }
        else
        {
            Console.WriteLine("Wrong hash type");
            return false;
        }

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

    public static void AvalancheEffect(int pairCount, int stringLength, string hashType)
    {
        int minBitDiff = int.MaxValue, maxBitDiff = int.MinValue, totalBitDiff = 0;
        int minHexDiff = int.MaxValue, maxHexDiff = int.MinValue, totalHexDiff = 0;

        float bitLenght = 256f, hexLenght = 64f;

        for (int i = 0; i < pairCount; i++)
        {
            string baseStr = GenerateRandomString(stringLength);
            char[] arr = baseStr.ToCharArray();
            int pos = new Random().Next(stringLength);
            arr[pos] = arr[pos] == 'A' ? 'B' : 'A'; // Change one symbol
            string modStr = new string(arr);

            string hash1 = Hashing.HashString(baseStr);
            string hash2 = Hashing.HashString(modStr);

            if (hashType == "gab")
            {
                hash1 = Hashing.HashString(baseStr);
                hash2 = Hashing.HashString(modStr);
            }
            else if (hashType == "MD5")
            {
                hash1 = GetMd5Hash(baseStr);
                hash2 = GetMd5Hash(modStr);
            }
            else if (hashType == "SHA1")
            {
                hash1 = GetSha1(baseStr);
                hash2 = GetSha1(modStr);
            }
            else if (hashType == "SHA256")
            {
                hash1 = GetSha256(baseStr);
                hash2 = GetSha256(modStr);
            }
            else if (hashType == "titoAI")
            {
                hash1 = StormHash.ComputeHash(baseStr);
                hash2 = StormHash.ComputeHash(modStr);
            }
            else if (hashType == "tito")
            {
                hash1 = Tito.Mixing(baseStr);
                hash2 = Tito.Mixing(modStr);
            }
            else
            {
                Console.WriteLine("Wrong hash type");
                return;
            }

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

        File.AppendAllText("tyrimas/avalancheResults.txt", $"{hashType}:\n Bit difference: min={minBitDiff/bitLenght*100}, max={maxBitDiff/bitLenght*100}, avg={(double)totalBitDiff / pairCount/bitLenght*100} \n Hex difference: min={minHexDiff/hexLenght*100}, max={maxHexDiff/hexLenght*100}, avg={(double)totalHexDiff / pairCount/hexLenght*100} \n");
    }

    static byte[] HexStringToBytes(string hex)
    {
        int len = hex.Length;
        byte[] bytes = new byte[len / 2];
        for (int i = 0; i < len; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }

    static string GetMd5Hash(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }

    static string GetSha1(string input)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha1.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }

    static string GetSha256(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}