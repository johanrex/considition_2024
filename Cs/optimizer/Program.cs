using Newtonsoft.Json;
using optimizer;
using optimizer.Models;
using System.Text;

//string gameUrl = "https://api.considition.com/";
string gameUrl = "http://localhost:8080/";
string apiKey = "05ae5782-1936-4c6a-870b-f3d64089dcf5";
string mapFile = "map.json";

void PrettyPrintJson(object obj)
{
    string prettyJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
    Console.WriteLine(prettyJson);
}


MapData GetMap(string mapFilename)
{
    string mapDataText = File.ReadAllText(mapFilename);
    var map = JsonConvert.DeserializeObject<MapData>(mapDataText);
    return map;
}


async Task<GameResponse> SubmitGame(GameInput input)
{
    HttpRequestMessage request = new();
    request.Method = HttpMethod.Post;
    request.RequestUri = new Uri(gameUrl + "game", UriKind.Absolute);
    request.Headers.Add("x-api-key", apiKey);

    Console.WriteLine("Request payload:");
    PrettyPrintJson(input);

    request.Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

    HttpClient client = new();
    client.BaseAddress = new Uri(gameUrl, UriKind.Absolute);

    var res = client.Send(request);
    Console.WriteLine("");
    Console.WriteLine(res.StatusCode);
    var responsePayload = await res.Content.ReadAsStringAsync();

    Console.WriteLine("Response:");
    PrettyPrintJson(JsonConvert.DeserializeObject(responsePayload));

    GameResponse gameResponse = JsonConvert.DeserializeObject<GameResponse>(responsePayload);

    return gameResponse;
}

//TODO hardcoded from json file
var personalities = PersonalityUtils.GetHardcodedPersonalities();

MapData map = GetMap(mapFile);


GameInput input = new()
{
    MapName = map.name,
    Proposals = new(),
    Iterations = new()
};


foreach (Customer customer in map.customers)
{
    //make sure all personality types we get from the map are known in the personalities dictionary
    if (!personalities.ContainsKey(customer.personality))
    {
        throw new KeyNotFoundException($"Personality {customer.personality} not found in the personalities dictionary.");
    }

    var personality = personalities[customer.personality];

    //var proposal = new CustomerLoanRequestProposal()
    //{
    //    CustomerName = customer.name,
    //    MonthsToPayBackLoan = map.gameLengthInMonths,
    //    YearlyInterestRate = personality.MaxInterestRate
    //};

    input.Proposals.Add(new CustomerLoanRequestProposal()
    {
        CustomerName = customer.name,
        MonthsToPayBackLoan = map.gameLengthInMonths,
        YearlyInterestRate = 0.1m, //TODO put maximum interest rate here, take from personality
    });
}

Random random = new Random();

string[] actionTypes = { "Award", "Skip" };
string[] awardTypes = { "IkeaFoodCoupon", "IkeaDeliveryCheck", "IkeaCheck", "GiftCard", "HalfInterestRate", "NoInterestRate" };


for (int i = 0; i < map.gameLengthInMonths; i++)
{
    //var x = new CustomerActionIteration();
    //input.Iterations.Add(x);
    Dictionary<string, CustomerAction> t = new();
    input.Iterations.Add(t);
    foreach (var customer in map.customers)
    {
        string randomType = actionTypes[random.Next(actionTypes.Length)];
        string randomAward = randomType == "Skip" ? "None" : awardTypes[random.Next(awardTypes.Length)];
        t.Add(customer.name, new CustomerAction
        {
            Type = randomType,
            Award = randomAward
        });
    }
}


var gameResponse = SubmitGame(input);

Console.WriteLine("Done.");