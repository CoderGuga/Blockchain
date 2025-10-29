using System;

namespace BlockchainSimulation
{
    /// bloko antraste su meta informacija
    public class BlockHeader
    {
        public string PrevBlockHash { get; set; }
        public DateTime Timestamp { get; set; }
        public string Version { get; set; }
        public string MerkleRootHash { get; set; }
        public long Nonce { get; set; }
        public int DifficultyTarget { get; set; }

        
        /// Konstruktorius su pradinėmis reikšmėmis
        
        public BlockHeader(string prevBlockHash, string merkleRootHash, int difficultyTarget = 3)
        {
            PrevBlockHash = prevBlockHash ?? "0";
            Timestamp = DateTime.UtcNow;
            Version = "1.0";
            MerkleRootHash = merkleRootHash ?? throw new ArgumentNullException(nameof(merkleRootHash));
            Nonce = 0;
            DifficultyTarget = difficultyTarget;
        }

        
        /// Apskaičiuoja header hash
        
        public string CalculateHash()
        {
            string data = $"{PrevBlockHash}{Timestamp.Ticks}{Version}{MerkleRootHash}{Nonce}{DifficultyTarget}";
            return TitoAi.Hash(data);
        }

        
        /// Patikrina ar hash atitinka difficulty target
        public bool HashMeetsDifficulty(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length < DifficultyTarget)
                return false;

            for (int i = 0; i < DifficultyTarget; i++)
            {
                if (hash[i] != '0')
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            return $@"
        Previous Hash:  {PrevBlockHash}
        Timestamp:      {Timestamp:yyyy-MM-dd HH:mm:ss.fff}
        Version:        {Version}
        Merkle Root:    {MerkleRootHash}
        Nonce:          {Nonce}
        Difficulty:     {DifficultyTarget} leading zeros";
        }
    }
}