using System;
using rot13;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter text to be encoded with ROT13:");
        string input = Console.ReadLine();
        string encoded = Rot13Encoder.Encode(input);
        Console.WriteLine($"Encoded: {encoded}");
    }
}
