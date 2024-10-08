using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        var key = GenerateSymmetricSecurityKey();
        Console.WriteLine($"Generated Key: {Convert.ToBase64String(key.Key)}");
    }

    static SymmetricSecurityKey GenerateSymmetricSecurityKey()
    {
        var randomBytes = new byte[32]; // 256-bit key
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return new SymmetricSecurityKey(randomBytes);
    }
}
