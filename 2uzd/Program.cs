using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BlockchainSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            PrintHeader();
            
            // Režimo pasirinkimas
            Console.WriteLine("\nSelect mode:");
            Console.WriteLine("1. Test mode (50 users, 500 transactions, difficulty 3)");
            Console.WriteLine("2. Full mode (1000 users, 10000 transactions, difficulty 3)");
            Console.WriteLine("3. Custom mode");
            Console.Write("\nYour choice (1-3): ");
            
                string? choice = Console.ReadLine();
            
            int userCount, txCount, difficulty, txPerBlock;
            
            switch (choice)
            {
                case "1":
                    userCount = 50;
                    txCount = 500;
                    difficulty = 3;
                    txPerBlock = 100;
                    break;
                case "2":
                    userCount = 1000;
                    txCount = 10000;
                    difficulty = 3;
                    txPerBlock = 100;
                    break;
                case "3":
                    Console.Write("Number of users: ");
                    userCount = int.Parse(Console.ReadLine() ?? "100");
                    Console.Write("Number of transactions: ");
                    txCount = int.Parse(Console.ReadLine() ?? "1000");
                    Console.Write("Difficulty (leading zeros): ");
                    difficulty = int.Parse(Console.ReadLine() ?? "3");
                    Console.Write("Transactions per block: ");
                    txPerBlock = int.Parse(Console.ReadLine() ?? "100");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Using test mode.");
                    userCount = 50;
                    txCount = 500;
                    difficulty = 3;
                    txPerBlock = 100;
                    break;
            }
            
            RunSimulation(userCount, txCount, difficulty, txPerBlock);
            
            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }

        static void RunSimulation(int userCount, int txCount, int difficulty, int txPerBlock)
        {
            Stopwatch totalTimer = Stopwatch.StartNew();
            
            try
            {
                // 1. Generuoti vartotojus
                List<User> users = DataGenerator.GenerateUsers(userCount);
                
                // 2. Sukurti blockchain
                Console.WriteLine($"\n⛓️  Initializing Blockchain (Difficulty: {difficulty}, TX/Block: {txPerBlock})...");
                Blockchain blockchain = new Blockchain(difficulty, txPerBlock);
                
                // 3. Registruoti vartotojus
                Console.WriteLine("\n📝 Registering users to blockchain...");
                foreach (var user in users)
                {
                    blockchain.AddUser(user);
                }
                Console.WriteLine($"✓ Registered {users.Count} users");
                
                // 4. Generuoti transakcijas
                List<Transaction> transactions = DataGenerator.GenerateTransactions(users, txCount);
                DataGenerator.ShuffleTransactions(transactions);
                
                // 5. Pridėti transakcijas
                Console.WriteLine("\n📤 Adding transactions to blockchain...");
                int addedTransactions = 0;
                int failedTransactions = 0;
                
                foreach (var tx in transactions)
                {
                    if (blockchain.AddTransaction(tx))
                        addedTransactions++;
                    else
                        failedTransactions++;
                    
                    if ((addedTransactions + failedTransactions) % 1000 == 0)
                    {
                        Console.Write($"\r   Progress: {addedTransactions + failedTransactions}/{transactions.Count}");
                    }
                }
                
                Console.WriteLine($"\n✓ Added {addedTransactions} transactions to pending pool");
                if (failedTransactions > 0)
                    Console.WriteLine($"   ⚠️  Failed to add {failedTransactions} transactions");
                
                // 6. Kasti blokus
                Console.WriteLine("\n" + new string('═', 64));
                Console.WriteLine("              STARTING BLOCKCHAIN MINING");
                Console.WriteLine(new string('═', 64));
                
                int blocksMined = 0;
                Stopwatch miningTimer = Stopwatch.StartNew();
                
                while (blockchain.PendingTransactions.Count > 0)
                {
                    blockchain.MineNextBlock();
                    blocksMined++;
                    
                    if (blocksMined % 5 == 0 || blockchain.PendingTransactions.Count < txPerBlock * 5)
                    {
                        Console.WriteLine($"\n📊 Progress: {blocksMined} blocks mined, {blockchain.PendingTransactions.Count} transactions remaining");
                    }
                }
                
                miningTimer.Stop();
                Console.WriteLine("\n" + new string('═', 64));
                Console.WriteLine("              MINING COMPLETED");
                Console.WriteLine(new string('═', 64));
                Console.WriteLine($"⏱️  Total mining time: {miningTimer.Elapsed.TotalSeconds:F2}s");
                Console.WriteLine($"📦 Blocks mined: {blocksMined}");
                Console.WriteLine($"⚡ Average time per block: {miningTimer.Elapsed.TotalSeconds / blocksMined:F2}s");
                
                // 7. Validacija
                Console.WriteLine("\n🔍 Validating blockchain...");
                bool isValid = blockchain.IsValid();
                if (isValid)
                {
                    Console.WriteLine("✓ Blockchain is valid!");
                }
                else
                {
                    Console.WriteLine("❌ Blockchain validation failed!");
                }
                
                // 8. Statistika
                blockchain.PrintStatistics();
                
                // 9. Rodyti blokus
                Console.WriteLine("\n📋 Displaying sample blocks...");
                
                if (blockchain.Chain.Count > 0)
                {
                    Console.WriteLine("\n--- GENESIS BLOCK ---");
                    Console.WriteLine(blockchain.Chain[0].ToString());
                }
                
                if (blockchain.Chain.Count > 1)
                {
                    Console.WriteLine("\n--- FIRST MINED BLOCK ---");
                    Console.WriteLine(blockchain.Chain[1].ToString());
                }
                
                if (blockchain.Chain.Count > 2)
                {
                    Console.WriteLine("\n--- LAST BLOCK ---");
                    Console.WriteLine(blockchain.Chain[blockchain.Chain.Count - 1].ToString());
                }
                
                // 10. Balansai
                Console.WriteLine("\n💰 Sample user balances (first 10):");
                int displayCount = Math.Min(10, users.Count);
                for (int i = 0; i < displayCount; i++)
                {
                    var user = users[i];
                    Console.WriteLine($"   {user.Name,-25} Balance: {user.Balance,15:F2}");
                }
                
                totalTimer.Stop();
                
                // 11. Finalo ataskaita
                Console.WriteLine("\n" + new string('═', 64));
                Console.WriteLine("              SIMULATION COMPLETED");
                Console.WriteLine(new string('═', 64));
                Console.WriteLine($"⏱️  Total execution time: {totalTimer.Elapsed.TotalSeconds:F2}s");
                Console.WriteLine($"👥 Users: {userCount}");
                Console.WriteLine($"💸 Transactions generated: {txCount}");
                Console.WriteLine($"✅ Transactions processed: {addedTransactions}");
                Console.WriteLine($"📦 Blocks created: {blockchain.Chain.Count}");
                Console.WriteLine($"⚙️  Difficulty: {difficulty} leading zeros");
                Console.WriteLine($"✓  Blockchain valid: {(isValid ? "Yes" : "No")}");
                Console.WriteLine(new string('═', 64));
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        static void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine(@"
╔════════════════════════════════════════════════════════════════╗
║                                                                ║
║        🔗 BLOCKCHAIN SIMULATION v0.1 🔗                       ║
║                                                                ║
║        Centralized Blockchain Implementation                   ║
║        Using TitoAi Hash Function                             ║
║                                                                ║
╚════════════════════════════════════════════════════════════════╝
");
        }
    }
}