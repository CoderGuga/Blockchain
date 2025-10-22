using System;

class Program
{
    static void Main()
    {
        var user = new User();
        Console.WriteLine($"Name: {user.name}");
        Console.WriteLine($"PublicKey: {user.publicKey}");
        Console.WriteLine($"Balance: {user.balance}");
    }
}