using System;
using System.Collections.Generic;
using System.Linq;

namespace BlockchainSimulation
{
    /// Generuoja atsitiktinius duomenis simuliacijai
    public class DataGenerator
    {
        private static readonly Random random = new Random();
        
        private static readonly string[] firstNames = {
            "Jonas", "Petras", "Antanas", "Mindaugas", "Tomas", "Darius", "Mantas", "Lukas", "Matas", "Rokas",
            "Marija", "Ona", "Rasa", "Gintare", "Agne", "Laura", "Ieva", "Kristina", "Ausra", "Ramune",
            "Andrius", "Vytautas", "Valdas", "Kestutis", "Sarunas", "Gediminas", "Algirdas", "Rytis", "Linas", "Arturas"
        };
        
        private static readonly string[] lastNames = {
            "Jonaitis", "Petraitis", "Kazlauskas", "Stankevicius", "Jankauskas", "Vasiliauskas", "Paulauskas",
            "Grigas", "Butkus", "Zukauskas", "Navickas", "Adamonis", "Mazeika", "Balciunas", "Ramanauskas",
            "Savickas", "Urbonas", "Stonkus", "Kavaliauskas", "Adomaitis", "Klimavicius", "Kacinskas"
        };

        /// Generuoja vartotojus naudodama User klasę
        public static List<User> GenerateUsers(int count = 1000)
        {
            Console.WriteLine($"\n👥 Generating {count} users...");
            List<User> users = new List<User>(count);
            HashSet<string> usedNames = new HashSet<string>();

            for (int i = 0; i < count; i++)
            {
                string name;
                do
                {
                    string firstName = firstNames[random.Next(firstNames.Length)];
                    string lastName = lastNames[random.Next(lastNames.Length)];
                    name = $"{firstName} {lastName}";
                    
                    if (usedNames.Contains(name))
                    {
                        name = $"{firstName} {lastName} {random.Next(1, 100)}";
                    }
                } while (usedNames.Contains(name));

                usedNames.Add(name);

                // Atsitiktinis balansas 100 - 1,000,000
                decimal balance = (decimal)(random.Next(100, 1000000) + random.NextDouble());
                
                User user = new User(name, balance);
                users.Add(user);

                // Progress
                if ((i + 1) % 100 == 0 || i == count - 1)
                {
                    int percentage = (int)(((double)(i + 1) / count) * 100);
                    Console.Write($"\r   Progress: [{new string('█', percentage / 5)}{new string('░', 20 - percentage / 5)}] {percentage}%");
                }
            }

            Console.WriteLine($"\n✓ Generated {count} users with total balance: {users.Sum(u => u.Balance):F2}");
            return users;
        }

        /// Generuoja transakcijas naudodama Transaction klasę
        public static List<Transaction> GenerateTransactions(List<User> users, int count = 10000)
        {
            Console.WriteLine($"\n💸 Generating {count} transactions...");
            
            if (users.Count < 2)
                throw new ArgumentException("Need at least 2 users to generate transactions");

            List<Transaction> transactions = new List<Transaction>(count);
            int validTransactions = 0;
            int invalidTransactions = 0;

            for (int i = 0; i < count; i++)
            {
                try
                {
                    // Atsitiktinai pasirinkti siuntėją ir gavėją
                    User sender = users[random.Next(users.Count)];
                    User receiver;
                    
                    do
                    {
                        receiver = users[random.Next(users.Count)];
                    } while (receiver.PublicKey == sender.PublicKey);

                    // Atsitiktinė suma (1-10% balanso, max 50000)
                    decimal maxAmount = Math.Min(sender.Balance * 0.1m, 50000);
                    if (maxAmount < 1)
                        maxAmount = 1;

                    decimal amount = (decimal)(random.NextDouble() * (double)maxAmount) + 0.01m;
                    amount = Math.Round(amount, 2);

                    // Patikrinti balansą
                    if (sender.Balance >= amount)
                    {
                        Transaction transaction = new Transaction(
                            sender.PublicKey,
                            receiver.PublicKey,
                            amount
                        );
                        
                        transactions.Add(transaction);
                        validTransactions++;
                    }
                    else
                    {
                        invalidTransactions++;
                    }
                }
                catch
                {
                    invalidTransactions++;
                }

                // Progress
                if ((i + 1) % 1000 == 0 || i == count - 1)
                {
                    int percentage = (int)(((double)(i + 1) / count) * 100);
                    Console.Write($"\r   Progress: [{new string('█', percentage / 5)}{new string('░', 20 - percentage / 5)}] {percentage}%");
                }
            }

            Console.WriteLine($"\n✓ Generated {validTransactions} valid transactions");
            if (invalidTransactions > 0)
                Console.WriteLine($"   ⚠️  Skipped {invalidTransactions} invalid transactions (insufficient balance)");
            
            decimal totalVolume = transactions.Sum(t => t.Amount);
            Console.WriteLine($"   Total transaction volume: {totalVolume:F2}");

            return transactions;
        }


        /// Išmaišo transakcijas
        public static void ShuffleTransactions(List<Transaction> transactions)
        {
            int n = transactions.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Transaction temp = transactions[i];
                transactions[i] = transactions[j];
                transactions[j] = temp;
            }
        }
    }
}