using Newtonsoft.Json;
using optimizer.Models.Pocos;
using System;

MapData GetMap(string mapFilename)
{
    string mapDataText = File.ReadAllText(mapFilename);
    var map = JsonConvert.DeserializeObject<MapData>(mapDataText);
    return map;
}

void WriteMap(MapData map, string mapFilename)
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
            name = "Customer" + i,
            personality = personalities[random.Next(personalities.Count)],
            capital = random.Next(1_000_001), // 1,000,001
            income = random.Next(50_000), // 50,000
            monthlyExpenses = random.Next(50_000), // 50,000
            numberOfKids = random.Next(8),
            homeMortgage = random.Next(100_000, 10_000_000), // 100,000 to 10,000,000
            hasStudentLoan = random.Next(2) == 1,
            loan = new Loan
            {
                product = "Home Mortgage",
                amount = random.Next(5_000_000), // 5,000,000
                environmentalImpact = random.Next(200) // 200
            }
        };
        customers.Add(customer);
    }

    //create a new MapData object
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
