using Newtonsoft.Json;
using optimizer;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System.Text;

//string gameUrl = "https://api.considition.com/";
string gameUrl = "http://localhost:8080/";
string apiKey = "05ae5782-1936-4c6a-870b-f3d64089dcf5";
//string mapFile = "map_10000.json";
string mapFile = "map.json";

/*
///////////////////////////////////////////////////////////////////
//Here comes the meat.
///////////////////////////////////////////////////////////////////
*/


GameUtils gameUtils = new GameUtils(gameUrl, apiKey);

//Read the map with all the customers
MapData map = GameUtils.GetMap(mapFile);

//TODO infer personalities instead.
var personalities = PersonalityUtils.GetHardcodedPersonalities();
if (!PersonalityUtils.HasKnownPersonalities(map, personalities))
{
    throw new Exception("Map contains unknown personalities.");
}
if (!PersonalityUtils.HasKnownInterestRates(personalities))
{
    throw new Exception("Some personalities have unknown interest rates.");
}


//Let's test simulated annealing
var customer = map.customers[0];
var customerName = customer.name;
var personality = PersonalityUtils.StringToEnum(customer.personality);
var personalitySpec = personalities[personality];
var acceptedMaxInterest = personalitySpec.AcceptedMaxInterest ?? 0.0;
var acceptedMinInterest = personalitySpec.AcceptedMinInterest ?? 0.0;
var startYearlyInterestRate = (acceptedMaxInterest - acceptedMinInterest)/2 + acceptedMinInterest;
var startMonthsToPayBackLoan = map.gameLengthInMonths/2;
var maxMonthsToPayBackLoan = 50 * 12;
var initialTemperature = 1000.0;
var coolingRate = 0.003;
var maxIterations = 1000;

SimulatedAnnealing anneal = new SimulatedAnnealing(
    gameUtils, 
    map.name, 
    map.gameLengthInMonths, 
    customerName, 
    startYearlyInterestRate, 
    startMonthsToPayBackLoan, 
    acceptedMinInterest, 
    acceptedMaxInterest, 
    maxMonthsToPayBackLoan);

(var optimalInterestRate, var optimalMonthsToPayBackLoan) = anneal.Run(
    startYearlyInterestRate,
    startMonthsToPayBackLoan,
    initialTemperature,
    coolingRate,
    maxIterations,
    acceptedMinInterest,
    acceptedMaxInterest,
    0,
    maxMonthsToPayBackLoan);

Console.WriteLine($"Customer name: {customerName}, optimalInterestRate: {optimalInterestRate}, optimalMonthsToPayBackLoan: {optimalMonthsToPayBackLoan}.");



//var monthsToPayBackLoan = 14;
//var input = LoanUtils.CreateSingleCustomerGameInput(map.name, map.gameLengthInMonths, map.customers[0].name, yearlyInterestRate, monthsToPayBackLoan);
//var gameResponse = await gameUtils.SubmitGame(input);

//foreach (var customer in map.customers)
//{
//    (var totalPayment, var proposal) = LoanUtils.FindOptimalLoanProposal(map, customer, personalities);

//    if(proposal != null)
//        input.Proposals.Add(proposal);
//}

/*
Random random = new Random();

string[] actionTypes = { "Award", "Skip" };
string[] awardTypes = { "IkeaFoodCoupon", "IkeaDeliveryCheck", "IkeaCheck", "GiftCard", "HalfInterestRate", "NoInterestRate" };

// Extract customer names from the proposals
var customerNames = input.Proposals.Select(p => p.CustomerName).ToList();

for (int i = 0; i < map.gameLengthInMonths; i++)
{
    Dictionary<string, CustomerAction> t = new();
    input.Iterations.Add(t);
    foreach (var customerName in customerNames)
    {
        string randomType = actionTypes[random.Next(actionTypes.Length)];
        string randomAward = randomType == "Skip" ? "None" : awardTypes[random.Next(awardTypes.Length)];
        t.Add(customerName, new CustomerAction
        {
            Type = randomType,
            Award = randomAward
        });
    }
}
*/

//var gameResponse = await SubmitGame(input);


Console.WriteLine("Done.");