using System.Security.Cryptography;
using System.Text;

// cia userio klase, kuri auto pasibuildina su User() konstruktoriumi
public class User
{
    public static List<User> users = new(); //cia irgi, viesa visu egzistuojanciu useriu duombaze
    string name;
    string publicKey;

    public User()
    {
        name = GenerateRandomName(5, 10);
        publicKey = GenPublicKey();
        new UTXO(publicKey, RandomNumberGenerator.GetInt32(100, 1000000));
        users.Add(this);
    }


    public void MakeTransaction(string receiver, int amount)
    {
        if (amount > GetBalance())
            Console.WriteLine("Bro, you poor");
        else
        {
            Transaction.CreateOrNull(publicKey, receiver, amount, FindUTXOs(amount));
        }
    }


//accesoriai

    public string GetName() => name;
    public string GetPublicKey() => publicKey;
    public int GetBalance()
    {
        int balance = 0;
        foreach (UTXO uTXO in GetUnspentUTXOs())
        {
            balance += uTXO.GetAmount();
        }
        return balance;
    }
    public List<UTXO> GetOwnedUTXOs() => UTXO.UTXOs.Where(u => u.GetOwnerKey() == publicKey).ToList();
    public List<UTXO> GetUnspentUTXOs() => UTXO.UTXOs.Where(u => u.GetOwnerKey() == publicKey && u.IsUnspent()).ToList();


    //helper func
    private readonly char[] chars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    private string GenerateRandomName(int min, int max)
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

    private string GenPublicKey()
    {
        using var rsa = RSA.Create(2048);
        var publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
        return publicKey;
    }

    private List<UTXO> FindUTXOs(int amount)
    {
        int currentAmount = 0;
        List<UTXO> usedUTXOs = new();

        foreach (UTXO uTXO in GetUnspentUTXOs())
        {
            currentAmount += uTXO.GetAmount();
            usedUTXOs.Add(uTXO);

            if (currentAmount >= amount)
                return usedUTXOs;
        }
        
        return usedUTXOs;
    }
}