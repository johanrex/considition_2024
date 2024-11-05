Console.WriteLine("foo");

/*
using Newtonsoft.Json;
using System;
using Common.Models;

void WriteMap(Map map, string mapFilename)
{
    string prettyJson = JsonConvert.SerializeObject(map, Formatting.Indented);
    File.WriteAllText(mapFilename, prettyJson);
}

void GenerateMap(int customerCount)
{
    var personalities = new List<string>
    {
        "Conservative",
        "RiskTaker",
        "Innovative",
        "Practical",
        "Spontaneous"
    };

    var random = new Random();
    var customers = new List<Customer>();

    for (int i = 0; i < customerCount; i++)
    {
        var customer = new Customer
        {
            Name = "Customer" + i,
            Personality = personalities[random.Next(personalities.Count)],
            Capital = random.Next(1_000_001), // 1,000,001
            Income = random.Next(50_000), // 50,000
            MonthlyExpenses = random.Next(50_000), // 50,000
            NumberOfKids = random.Next(8),
            HomeMortgage = random.Next(100_000, 10_000_000), // 100,000 to 10,000,000
            HasStudentLoan = random.Next(2) == 1,
            Loan = new Loan
            {
                Product = "Home Mortgage",
                Amount = random.Next(5_000_000), // 5,000,000
                EnvironmentalImpact = random.Next(200) // 200
            }
        };
        customers.Add(customer);
    }

    //create a new Map object
    MapData map = new MapData
    {
        name = $"{customerCount} customers",
        budget = 100_000_000, // 100,000,000
        gameLengthInMonths = 36,
        customers = customers.ToArray()
    };


    var filename = $"map_{customerCount}.json";
    //var map = GetMap("map.json");
    WriteMap(map, filename);
    Console.WriteLine("Wrote: " + filename);
}

GenerateMap(50);
GenerateMap(100);
GenerateMap(1000);
GenerateMap(10000);


Console.WriteLine("Done.");

*/