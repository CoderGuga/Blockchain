namespace BlockchainSimulation
{

    public class UTXO
    {

        public static List<UTXO> UTXOs = new();     //zodziu, cia yra yra egzistuojantys UTXOs, kurie yra prieinami visam pasauliui (todel static)
        public static string CoinbaseTransactionID = "0000000000000000";

        //kiekvienas UTXO turi situs fields
        public string prevTXID { get; private set; }
        public int vout { get; private set; }
        public string ownerKey { get; private set; }
        public int amount { get; private set; }
        public bool unspent { get; private set; }

        public UTXO(string _ownerKey, int _amount, string _prevTXID, int _vout, bool _unspent = true)
        {
            prevTXID = _prevTXID;
            vout = _vout;
            ownerKey = _ownerKey;
            amount = _amount;
            unspent = _unspent;
            UTXOs.Add(this);
        }
        public void Spend() => unspent = false;
        
        private void CreateID()
        {
            
        }
    }
}