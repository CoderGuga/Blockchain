public class Transaction
{
    public static List<Transaction> confirmedTransactions = new(), unconfirmedTransactions = new(); // o cia visos egzistuojancio transakcijos (confirmed tos, kurios yra idetos i bloka, uncomfirmed, kurios neidetos dar)
    int transaction_id;
    string sender;
    string receiver;
    int amount;
    List<UTXO> inputs, outputs;

    private Transaction(string _sender, string _receiver, int _amount, List<UTXO> _inputs)
    {
        //konstruktorius automatiskai viska sudeda, paspendina visus sunaudotus UTXOs ir grazina dar, jei yra per daug
        transaction_id = confirmedTransactions.Count + unconfirmedTransactions.Count;
        sender = _sender;
        receiver = _receiver;
        amount = _amount;
        inputs = _inputs;

        int amountSpent = CountAmount(_inputs);
        outputs = new List<UTXO>{ new UTXO(_receiver, _amount) };
        if (_amount < amountSpent)
            outputs.Add(new UTXO(sender, amountSpent - amount));

        foreach (UTXO uTXO in _inputs)
            uTXO.Spend();

        unconfirmedTransactions.Add(this);

        Console.WriteLine($"Transaction ID: {transaction_id}\nAmount: {amount}\nInput Count: {inputs.Count}\nInputs: {UXTOListToString(inputs)}\nOutput Count: {outputs.Count}\nOutputs: {UXTOListToString(outputs)}");
    }

        // sita arba CreateOrNull apacioj naudoti transakciju kurimui (priklausomai nuo to ar reikia bool, ar Transaction gauti)
    public static bool TryCreate(string _sender, string _receiver, int _amount, List<UTXO> _inputs, out Transaction? tx)
    {
        List<UTXO> unspentUTXOs = new();
        foreach (UTXO uTXO in _inputs)
        {
            if (uTXO.IsUnspent())
                unspentUTXOs.Add(uTXO);
        }
        
        tx = null;
        if (_inputs == null || _inputs.Count == 0) return false;

        int sum = CountAmount(unspentUTXOs);

        if (sum < _amount) return false;

        tx = new Transaction(_sender, _receiver, _amount, _inputs);
        return true;
    }

    // sita reikia vartoti vietoj construktoriaus, nes kitaip neimanoma patikrinti, ar ciuvakas turi pinigu isvis daryti tokia transakcija
    public static Transaction? CreateOrNull(string _sender, string _receiver, int _amount, List<UTXO> _inputs)
    {
        return TryCreate(_sender, _receiver, _amount, _inputs, out var tx) ? tx : null;
    }


    //cia pagalbines funkcijos, tai nesvarbu
    private static int CountAmount(List<UTXO> uTXOs)
    {
        int balance = 0;
        foreach (UTXO uTXO in uTXOs)
        {
            balance += uTXO.GetAmount();
        }
        return balance;
    }

    private string UXTOListToString(List<UTXO> uTXOs)
    {
        string output = "";
        foreach (UTXO uTXO in uTXOs)
        {
            output += uTXO.GetOwnerKey() + "\n\n";
        }
        return output;
    }

    private string UXTOListToID(List<UTXO> uTXOs)
    {
        string output = "";
        foreach (UTXO uTXO in uTXOs)
        {
            output += uTXO.GetId() + "\n";
        }
        return output;
    }

    // accessors (one-line)
    public int GetId() => transaction_id;
    public string GetSender() => sender;
    public string GetReceiver() => receiver;
    public int GetAmount() => amount;
    public List<UTXO> GetInputs() => inputs;
    public List<UTXO> GetOutputs() => outputs;
}