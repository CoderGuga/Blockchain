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
}