public class Transaction
{
    public static List<Transaction> confirmedTransactions = new(), unconfirmedTransactions = new();
    int transaction_id;
    string sender;
    string receiver;
    int amount;
    List<UTXO> inputs, outputs;

    public Transaction(string _sender, string _receiver, int _amount, List<UTXO> _inputs)
    {
        transaction_id = confirmedTransactions.Count + unconfirmedTransactions.Count - 1;
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
    }

        // TryCreate pattern: returns true and an instance when conditions met
    public static bool TryCreate(string _sender, string _receiver, int _amount, List<UTXO> _inputs, out Transaction tx)
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

    // Convenience factory that returns null on failure
    public static Transaction CreateOrNull(string _sender, string _receiver, int _amount, List<UTXO> _inputs)
    {
        return TryCreate(_sender, _receiver, _amount, _inputs, out var tx) ? tx : null;
    }

    private static int CountAmount(List<UTXO> uTXOs)
    {
        int balance = 0;
        foreach (UTXO uTXO in uTXOs)
        {
            balance += uTXO.GetAmount();
        }
        return balance;
    }

    // accessors (one-line)
    public int GetId() => transaction_id;
    public string GetSender() => sender;
    public string GetReceiver() => receiver;
    public int GetAmount() => amount;
    public List<UTXO> GetInputs() => inputs;
    public List<UTXO> GetOutputs() => outputs;
}