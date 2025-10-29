using System;
using System.Collections.Generic;
using System.Linq;

namespace BlockchainSimulation
{
    public class Transaction
    {
        public static List<Transaction> confirmedTransactions = new List<Transaction>();
        public static List<Transaction> unconfirmedTransactions = new List<Transaction>();
        
        public int TransactionId { get; private set; }
        public string Sender { get; private set; } = null!;
        public string Receiver { get; private set; } = null!;
        public int Amount { get; private set; }
        public List<UTXO> Inputs { get; private set; } = null!;
        public List<UTXO> Outputs { get; private set; } = null!;
        public bool IsValid { get; private set; }
    private bool _inputsReserved = false;
        
        public bool Validate() => ValidateTransaction();

        public Transaction(string sender, string receiver, int amount)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = amount;
            TransactionId = confirmedTransactions.Count + unconfirmedTransactions.Count;
            Inputs = new List<UTXO>();
            Outputs = new List<UTXO>();
            IsValid = true;
            unconfirmedTransactions.Add(this);
        }

        private Transaction(string sender, string receiver, int amount, List<UTXO> inputs)
            : this(sender, receiver, amount)
        {
            SetInputs(inputs);
            IsValid = ValidateTransaction();
        }

        /// Set transaction inputs and compute outputs (including change UTXO if needed).
        public void SetInputs(List<UTXO> inputs)
        {
            Inputs = inputs ?? new List<UTXO>();
            Outputs = new List<UTXO> { new UTXO(Receiver, Amount) };
            int amountSpent = CountAmount(Inputs);
            if (amountSpent > Amount)
                Outputs.Add(new UTXO(Sender, amountSpent - Amount));
            // Reset reservation flag when inputs change
            _inputsReserved = false;
            IsValid = ValidateTransaction();
        }

        /// Attempt to reserve (spend) the input UTXOs for this transaction.
        /// Returns true if all inputs were unspent and are now marked spent.
        public bool ReserveInputs()
        {
            if (Inputs == null || Inputs.Count == 0)
                return true;

            if (_inputsReserved)
                return true;

            // Check all inputs are still unspent
            foreach (var u in Inputs)
            {
                if (!u.IsUnspent())
                    return false;
            }

            // Mark them as spent
            foreach (var u in Inputs)
                u.Spend();

            _inputsReserved = true;
            return true;
        }

        /// Confirm transaction (move from unconfirmed to confirmed)
        public void Confirm()
        {
            if (unconfirmedTransactions.Contains(this))
            {
                unconfirmedTransactions.Remove(this);
                confirmedTransactions.Add(this);
            }
        }

        public static bool TryCreate(string sender, string receiver, int amount, List<UTXO> inputs, out Transaction tx)
        {
            tx = null!;
            if (inputs == null || inputs.Count == 0) return false;

            // Only consider currently unspent UTXOs from the provided list
            List<UTXO> unspentUTXOs = inputs.Where(u => u.IsUnspent()).ToList();
            int sum = CountAmount(unspentUTXOs);
            if (sum < amount) return false;

            tx = new Transaction(sender, receiver, amount);
            tx.SetInputs(unspentUTXOs);
            return true;
        }

        public static Transaction? CreateOrNull(string sender, string receiver, int amount, List<UTXO> inputs)
        {
            return TryCreate(sender, receiver, amount, inputs, out var tx) ? tx : null;
        }

        private bool ValidateTransaction()
        {
            if (string.IsNullOrEmpty(Sender) || string.IsNullOrEmpty(Receiver))
                return false;
                
            if (Amount <= 0)
                return false;

            if (Inputs != null && Inputs.Any())
            {
                var totalInput = CountAmount(Inputs);
                if (totalInput < Amount)
                    return false;

                // If inputs were already reserved by this transaction, allow them
                // (they will be marked spent). Otherwise ensure inputs are unspent.
                if (!_inputsReserved && Inputs.Any(i => !i.IsUnspent()))
                    return false;
            }

            return true;
        }

        private static int CountAmount(List<UTXO> utxos)
        {
            return utxos?.Sum(utxo => utxo.GetAmount()) ?? 0;
        }

        private string UXTOListToString(List<UTXO> utxos)
        {
            return string.Join("\n\n", utxos.Select(utxo => utxo.GetOwnerKey()));
        }

        private string UXTOListToID(List<UTXO> utxos)
        {
            return string.Join("\n", utxos.Select(utxo => utxo.GetId().ToString()));
        }
    }
}