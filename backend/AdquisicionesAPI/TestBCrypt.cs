using System;

class TestBCrypt 
{
    static void Main() 
    {
        var password = "admin123";
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        Console.WriteLine($"Generated Hash: {hash}");
        
        var seedHash = "$2a$11$vZ8qLKHPvXDKJX7M9wYl3.rO7ZB9yX7j/GKOmYJQqHzJqGEp5L8Eq";
        Console.WriteLine($"\nSeed Hash: {seedHash}");
        Console.WriteLine($"Seed Hash Verifies: {BCrypt.Net.BCrypt.Verify(password, seedHash)}");
        Console.WriteLine($"New Hash Verifies: {BCrypt.Net.BCrypt.Verify(password, hash)}");
    }
}
