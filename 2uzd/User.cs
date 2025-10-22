using System.Security.Cryptography;
using System.Text;

public class User
{
    public string name;
    public string publicKey;
    public int balance;

    public User()
    {
        name = GenerateRandomName(5, 10);
        publicKey = GenerateRandomName(32, 32);
        balance = RandomNumberGenerator.GetInt32(100, 1000000);
    }

    private static readonly char[] chars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    private static string GenerateRandomName(int min, int max)
    {
        max++;
        int length = RandomNumberGenerator.GetInt32(min, max); // 5..10 inclusive
        var sb = new StringBuilder(length);
        var bytes = new byte[length];
        RandomNumberGenerator.Fill(bytes);
        for (int i = 0; i < length; i++)
            sb.Append(chars[bytes[i] % chars.Length]);
        return sb.ToString();
    }
}