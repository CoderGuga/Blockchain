using System;


class Program
{
    static void Main()
    {
        User user = new User();
        Console.WriteLine($"Name: {user.GetName()}");
        Console.WriteLine($"PublicKey: {user.GetPublicKey()}");
        Console.WriteLine($"Balance: {user.GetBalance()}");
    }
}