using CsharpStarterkit;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

//string gameUrl = "https://api.considition.com/";
string gameUrl = "http://localhost:8080/";
string apiKey = "05ae5782-1936-4c6a-870b-f3d64089dcf5";

void PrettyPrintJson(object obj)
{
    string prettyJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
    Console.WriteLine(prettyJson);
}

 
HttpClient client = new();
client.BaseAddress = new Uri(gameUrl, UriKind.Absolute);

GameInput input = new()
{
    MapName = "Gothenburg",
    Proposals = new(),
    Iterations = new()
};

string mapFile = "Map.json";

string mapDataText = File.ReadAllText(mapFile);
MapData mapData = JsonConvert.DeserializeObject<MapData>(mapDataText);

foreach (Customer customer in mapData.customers)
{
    input.Proposals.Add(new CustomerLoanRequestProposal()
    {
        CustomerName = customer.name,
        MonthsToPayBackLoan = mapData.gameLengthInMonths,
        YearlyInterestRate = 0.1m,
    });
}

Random random = new Random();

string[] actionTypes = { "Award", "Skip" };
string[] awardTypes = { "IkeaFoodCoupon", "IkeaDeliveryCheck", "IkeaCheck", "GiftCard", "HalfInterestRate", "NoInterestRate" };

//This is so more data to optimize awards
//var awardsData = JsonConvert.DeserializeObject<AwardsRoot>(File.ReadAllText("Awards.json"));


for (int i = 0; i < mapData.gameLengthInMonths; i++)
{
    //var x = new CustomerActionIteration();
    //input.Iterations.Add(x);
    Dictionary<string, CustomerAction> t = new();
    input.Iterations.Add(t);
    foreach (var customer in mapData.customers)
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

HttpRequestMessage request = new();
request.Method = HttpMethod.Post;
request.RequestUri = new Uri(gameUrl + "game", UriKind.Absolute);
request.Headers.Add("x-api-key", apiKey);

Console.WriteLine("Request payload:");
PrettyPrintJson(input);

request.Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

var res = client.Send(request);
Console.WriteLine("");
Console.WriteLine(res.StatusCode);
var responsePayload = await res.Content.ReadAsStringAsync();

Console.WriteLine("Response:");
PrettyPrintJson(JsonConvert.DeserializeObject(responsePayload));

