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
}