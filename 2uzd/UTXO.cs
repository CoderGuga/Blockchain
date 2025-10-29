namespace BlockchainSimulation
{

    public class UTXO
    {

        public static List<UTXO> UTXOs = new();     //zodziu, cia yra yra egzistuojantys UTXOs, kurie yra prieinami visam pasauliui (todel static)

        //kiekvienas UTXO turi situs fields
        int id;
        string ownerKey;
        int amount;
        bool unspent;

        public UTXO(string _ownerKey, int _amount, bool _unspent = true)
        {
            id = UTXOs.Count;
            ownerKey = _ownerKey;
            amount = _amount;
            unspent = _unspent;
            UTXOs.Add(this);
        }

        public int GetId() => id;
        public string GetOwnerKey() => ownerKey;
        public int GetAmount() => amount;
        public bool IsUnspent() => unspent;
        public void Spend() => unspent = false;
    }
}