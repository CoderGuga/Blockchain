using System;
using System.Collections.Generic;
using System.Linq;

namespace BlockchainSimulation
{

    /// Pagrindinė Blockchain klasė    
    public class Blockchain
    {
        public List<Block> Chain { get; private set; }
        public List<Transaction> PendingTransactions { get; private set; }
        public Dictionary<string, User> Users { get; private set; }
        public int DifficultyTarget { get; set; }
        public int TransactionsPerBlock { get; set; }

        
        /// Konstruktorius su genesis bloku
        public Blockchain(int difficultyTarget = 3, int transactionsPerBlock = 100)
        {
            Chain = new List<Block>();
            PendingTransactions = new List<Transaction>();
            Users = new Dictionary<string, User>();
            DifficultyTarget = difficultyTarget;
            TransactionsPerBlock = transactionsPerBlock;

            CreateGenesisBlock();
        }

        
        /// Sukuria genesis bloka
        private void CreateGenesisBlock()
        {
            Console.WriteLine("\n🔷 Creating Genesis Block...");
            
            var genesisTransaction = Transaction.CreateGenesisTransaction();

            var genesisBlock = new Block(0, "0", new List<Transaction> { genesisTransaction }, DifficultyTarget);
            genesisBlock.Mine();
            Chain.Add(genesisBlock);

            Console.WriteLine("✓ Genesis Block created!");
        }

    
        /// Prideda vartotoja
        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            
            if (Users.ContainsKey(user.PublicKey))
                throw new InvalidOperationException("User with this public key already exists");

            Users[user.PublicKey] = user;
        }

        
        /// Prideda transakciją į laukiančių sąrašą
        public bool AddTransaction(Transaction transaction)
        {
            if (transaction == null || !transaction.Validate())
                return false;

            // Patikrinti siuntėją
            if (!Users.ContainsKey(transaction.Sender))
                return false;

            // Patikrinti gavėją
            if (!Users.ContainsKey(transaction.Receiver))
                return false;

            // Patikrinti balansą
            var sender = Users[transaction.Sender];
            if (!sender.HasSufficientBalance(transaction.Amount))
                return false;

            // Reserve inputs so they cannot be double-spent by other pending TXs
            if (!transaction.ReserveInputs())
                return false;

            PendingTransactions.Add(transaction);
            return true;
        }

        /// Iškasa naują bloką
        public void MineNextBlock()
        {
            if (PendingTransactions.Count == 0)
            {
                Console.WriteLine("\n⚠️  No pending transactions to mine.");
                return;
            }

            // Paima transakcijas
            int txCount = Math.Min(TransactionsPerBlock, PendingTransactions.Count);
            List<Transaction> transactionsToMine = PendingTransactions.Take(txCount).ToList();

            Console.WriteLine($"\n📦 Creating new block with {txCount} transactions...");

            // Gauti paskutinio bloko hash
            string prevHash = GetLatestBlock().Hash;
            int newIndex = Chain.Count;

            // Sukurti bloką
            Block newBlock = new Block(newIndex, prevHash, transactionsToMine, DifficultyTarget);
            
            // Iškasti
            newBlock.Mine();

            // Validuoti
            if (!newBlock.IsValid(prevHash))
            {
                Console.WriteLine("❌ Block validation failed!");
                return;
            }

            // Pridėti prie grandinės
            Chain.Add(newBlock);

            // Atnaujinti balansus
            UpdateBalances(transactionsToMine);

            // Pašalinti įtrauktas transakcijas
            PendingTransactions.RemoveRange(0, txCount);

            Console.WriteLine($"✓ Block #{newIndex} added to blockchain!");
            Console.WriteLine($"   Remaining pending transactions: {PendingTransactions.Count}");
        }

        /// Atnaujina balansus 
        private void UpdateBalances(List<Transaction> transactions)
        {
                foreach (var tx in transactions)
                {
                    // Transactions were already reserved (inputs spent) and outputs
                    // created when the transaction was accepted. Here we simply
                    // confirm them (move from unconfirmed to confirmed list).
                    tx.Confirm();
                }
        }

        
        /// Grąžina paskutinį bloką
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        /// Validuoja visą grandinę
        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (!currentBlock.IsValid(previousBlock.Hash))
                {
                    Console.WriteLine($"❌ Block #{i} is invalid!");
                    return false;
                }

                if (currentBlock.Header.PrevBlockHash != previousBlock.Hash)
                {
                    Console.WriteLine($"❌ Block #{i} is not properly linked!");
                    return false;
                }
            }

            return true;
        }

        public void PrintStatistics()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║               BLOCKCHAIN STATISTICS                            ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ Total Blocks:              {Chain.Count,-33}║");
            Console.WriteLine($"║ Total Users:               {Users.Count,-33}║");
            Console.WriteLine($"║ Pending Transactions:      {PendingTransactions.Count,-33}║");
            Console.WriteLine($"║ Difficulty Target:         {DifficultyTarget} leading zeros{"",-23}║");
            Console.WriteLine($"║ Transactions per Block:    {TransactionsPerBlock,-33}║");
            
            int totalTransactions = Chain.Sum(b => b.Transactions.Count) - 1;
            decimal totalVolume = Chain.Skip(1).Sum(b => b.GetTotalTransactionAmount());
            
            Console.WriteLine($"║ Total Transactions:        {totalTransactions,-33}║");
            Console.WriteLine($"║ Total Transaction Volume:  {totalVolume,-33:F2}║");
            
            if (totalTransactions > 0)
            {
                decimal avgTxPerBlock = (decimal)totalTransactions / (Chain.Count - 1);
                Console.WriteLine($"║ Avg Transactions/Block:    {avgTxPerBlock,-33:F2}║");
            }
            
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
        }

        /// Grąžina vartotojo balansą
        public decimal GetUserBalance(string publicKey)
        {
            return Users.ContainsKey(publicKey) ? Users[publicKey].Balance : 0;
        }
    }
}