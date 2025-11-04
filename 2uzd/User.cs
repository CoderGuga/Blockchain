using System.Security.Cryptography;
using System.Text;

namespace BlockchainSimulation
{
    // cia userio klase, kuri auto pasibuildina su User() konstruktoriumi
    public class User
    {
        public static List<User> users = new(); //cia irgi, viesa visu egzistuojanciu useriu duombaze
        string name;
        string publicKey;

        public User(string? name = null, decimal initialBalance = 0)
        {
            this.name = name ?? GenerateRandomName(5, 10);
            publicKey = GenPublicKey();
            
            // Create initial UTXO if balance provided or generate random
            var amount = initialBalance > 0 ? (int)initialBalance : RandomNumberGenerator.GetInt32(100, 1000000);
            new UTXO(publicKey, amount, UTXO.CoinbaseTransactionID, 0);
            
            users.Add(this);
        }


        public void MakeTransaction(string receiver, int amount)
        {
            if (amount > Balance)
                Console.WriteLine("Insufficient funds for transaction");
            else
            {
                Transaction.CreateOrNull(publicKey, receiver, amount, FindUTXOs(amount));
            }
        }


        //accesoriai

        public string Name => name;
        public string PublicKey => publicKey;
        public int Balance
        {
            get
            {
                int balance = 0;
                foreach (UTXO uTXO in UnspentUTXOs)
                {
                    balance += uTXO.amount;
                }
                return balance;
            }
        }
        public List<UTXO> OwnedUTXOs => UTXO.UTXOs.Where(u => u.ownerKey == publicKey).ToList();
        public List<UTXO> UnspentUTXOs => UTXO.UTXOs.Where(u => u.ownerKey == publicKey && u.unspent).ToList();


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

            foreach (UTXO uTXO in UnspentUTXOs)
            {
                currentAmount += uTXO.amount;
                usedUTXOs.Add(uTXO);

                if (currentAmount >= amount)
                    return usedUTXOs;
            }

            return usedUTXOs;
        }

        public bool HasSufficientBalance(int amount) => Balance >= amount;

        public void Credit(int amount) => new UTXO(publicKey, amount, UTXO.CoinbaseTransactionID, 0);
        
        public void Debit(int amount)
        {
            // Find UTXOs to spend
            var utxos = FindUTXOs(amount);
            if (utxos.Count == 0 || CountAmount(utxos) < amount)
                throw new InvalidOperationException("Insufficient balance");
                
            // Spend them
            foreach (var utxo in utxos)
                utxo.Spend();
        }

        private static int CountAmount(List<UTXO> utxos)
        {
            return utxos?.Sum(utxo => utxo.amount) ?? 0;
        }
    }
}