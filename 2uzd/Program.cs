using System;


class Program
{
    static void Main()
    {
        User user1 = new User();
        User user2 = new User();

        Console.WriteLine($"user1 money {user1.GetBalance()}, user2 money {user2.GetBalance()}");

        user1.MakeTransaction(user2.GetPublicKey(), user1.GetBalance() - 1);

        Console.WriteLine($"user1 money {user1.GetBalance()}, user2 money {user2.GetBalance()}, transaction {Transaction.unconfirmedTransactions[0].GetAmount()}");
    }
}