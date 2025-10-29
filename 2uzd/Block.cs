using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlockchainSimulation
{
    
    /// Reprezentuoja vieną bloką blockchain grandinėje
    
    public class Block
    {
        public int Index { get; private set; }
        public BlockHeader Header { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public string Hash { get; private set; }

        
        // Konstruktorius sukurti naują bloką
        public Block(int index, string prevBlockHash, List<Transaction> transactions, int difficultyTarget = 3)
        {
            if (transactions == null || transactions.Count == 0)
                throw new ArgumentException("Block must contain at least one transaction");

            Index = index;
            Transactions = new List<Transaction>(transactions);
            
            string merkleRoot = CalculateMerkleRoot();
            Header = new BlockHeader(prevBlockHash, merkleRoot, difficultyTarget);
            Hash = string.Empty;
        }

     
      // Apskaičiuoja Merkle Root 
       
        private string CalculateMerkleRoot()
        {
            if (Transactions.Count == 0)
                return TitoAi.Hash("empty");

            StringBuilder allTxHashes = new StringBuilder();
            foreach (var tx in Transactions)
            {
                allTxHashes.Append(tx.TransactionId);
            }
            
            return TitoAi.Hash(allTxHashes.ToString());
        }

       
        // Proof-of-Work kasimo procesas
     
        public void Mine()
        {
            Console.WriteLine($"\n⛏️  Mining Block #{Index}...");
            Console.WriteLine($"   Target: {new string('0', Header.DifficultyTarget)}...");
            
            DateTime startTime = DateTime.UtcNow;
            long attempts = 0;
            
            do
            {
                Header.Nonce++;
                Hash = Header.CalculateHash();
                attempts++;

                // Progreso rodymas kas 100000 bandymų
                if (attempts % 100000 == 0)
                {
                    Console.WriteLine($"   Attempts: {attempts:N0}, Current hash: {Hash.Substring(0, 16)}...");
                }

            } while (!Header.HashMeetsDifficulty(Hash));

            TimeSpan miningTime = DateTime.UtcNow - startTime;
            
            Console.WriteLine($"✓  Block mined successfully!");
            Console.WriteLine($"   Hash:     {Hash}");
            Console.WriteLine($"   Nonce:    {Header.Nonce:N0}");
            Console.WriteLine($"   Attempts: {attempts:N0}");
            Console.WriteLine($"   Time:     {miningTime.TotalSeconds:F2}s");
        }

    
        /// Validuoja bloko struktūrą
        public bool IsValid(string previousBlockHash)
        {
            // Patikrinti hash
            if (Hash != Header.CalculateHash())
                return false;

            // Patikrinti difficulty
            if (!Header.HashMeetsDifficulty(Hash))
                return false;

            // Patikrinti sąsają su ankstesniu bloku
            if (Header.PrevBlockHash != previousBlockHash)
                return false;

            // Patikrinti Merkle root
            string calculatedMerkleRoot = CalculateMerkleRoot();
            if (Header.MerkleRootHash != calculatedMerkleRoot)
                return false;

            // Validuoti transakcijas
            foreach (var tx in Transactions)
            {
                if (!tx.IsValid())
                    return false;
            }

            return true;
        }

    
        // Bendra transakcijų suma
        public decimal GetTotalTransactionAmount()
        {
            return Transactions.Sum(tx => tx.Amount);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\n╔════════════════════════════════════════════════════════════════╗");
            sb.AppendLine($"║ BLOCK #{Index,-58}║");
            sb.AppendLine($"╠════════════════════════════════════════════════════════════════╣");
            sb.AppendLine($"║ Hash: {Hash,-55}║");
            sb.AppendLine($"╠════════════════════════════════════════════════════════════════╣");
            sb.AppendLine($"║ HEADER:{"",-56}║");
            sb.AppendLine($"║   Previous Hash:  {Header.PrevBlockHash,-42}║");
            sb.AppendLine($"║   Timestamp:      {Header.Timestamp:yyyy-MM-dd HH:mm:ss.fff}                       ║");
            sb.AppendLine($"║   Version:        {Header.Version,-42}║");
            sb.AppendLine($"║   Merkle Root:    {Header.MerkleRootHash,-42}║");
            sb.AppendLine($"║   Nonce:          {Header.Nonce,-42}║");
            sb.AppendLine($"║   Difficulty:     {Header.DifficultyTarget} leading zeros{"",-36}║");
            sb.AppendLine($"╠════════════════════════════════════════════════════════════════╣");
            sb.AppendLine($"║ TRANSACTIONS: {Transactions.Count,-47}║");
            sb.AppendLine($"║ Total Amount: {GetTotalTransactionAmount(),-47:F2}║");
            
            // Rodyti pirmas 5 transakcijas
            int displayCount = Math.Min(5, Transactions.Count);
            for (int i = 0; i < displayCount; i++)
            {
                var tx = Transactions[i];
                string senderShort = tx.Sender.Length >= 8 ? tx.Sender.Substring(0, 8) : tx.Sender;
                string receiverShort = tx.Receiver.Length >= 8 ? tx.Receiver.Substring(0, 8) : tx.Receiver;
                sb.AppendLine($"║   {i + 1}. {senderShort}... → {receiverShort}... | {tx.Amount,-10:F2}║");
            }
            
            if (Transactions.Count > 5)
            {
                sb.AppendLine($"║   ... and {Transactions.Count - 5} more transactions{"",-37}║");
            }
            
            sb.AppendLine($"╚════════════════════════════════════════════════════════════════╝");
            
            return sb.ToString();
        }
    }
}