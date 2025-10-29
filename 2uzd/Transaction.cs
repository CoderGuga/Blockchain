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
        
        public bool Validate() => ValidateTransaction();

        public Transaction(string sender, string receiver, int amount)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = amount;
            TransactionId = confirmedTransactions.Count + unconfirmedTransactions.Count;
            Inputs = new List<UTXO>();
            Outputs = new List<UTXO> { new UTXO(receiver, amount) };
            IsValid = true;
            unconfirmedTransactions.Add(this);
        }

        private Transaction(string sender, string receiver, int amount, List<UTXO> inputs)
        {
            TransactionId = confirmedTransactions.Count + unconfirmedTransactions.Count;
            Sender = sender;
            Receiver = receiver;
            Amount = amount;
            Inputs = inputs ?? new List<UTXO>();

            if (Inputs.Count > 0)
            {
                int amountSpent = CountAmount(Inputs);
                Outputs = new List<UTXO> { new UTXO(Receiver, Amount) };
                if (Amount < amountSpent)
                    Outputs.Add(new UTXO(Sender, amountSpent - Amount));

                foreach (UTXO utxo in Inputs)
                    utxo.Spend();
            }
            else
            {
                Outputs = new List<UTXO> { new UTXO(Receiver, Amount) };
            }

            IsValid = ValidateTransaction();
            unconfirmedTransactions.Add(this);
        }

        public static bool TryCreate(string sender, string receiver, int amount, List<UTXO> inputs, out Transaction tx)
        {
            List<UTXO> unspentUTXOs = new();
            foreach (UTXO utxo in inputs)
            {
                if (utxo.IsUnspent())
                    unspentUTXOs.Add(utxo);
            }

                tx = null!;
            if (inputs == null || inputs.Count == 0) return false;

            int sum = CountAmount(unspentUTXOs);

            if (sum < amount) return false;

            tx = new Transaction(sender, receiver, amount, inputs);
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

                if (Inputs.Any(i => !i.IsUnspent()))
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