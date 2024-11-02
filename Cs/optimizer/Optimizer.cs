using Newtonsoft.Json;
using optimizer;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System.Text;

string gameUrlRemote = "https://api.considition.com/";
string gameUrlLocal = "http://localhost:8080/";

string apiKey = "05ae5782-1936-4c6a-870b-f3d64089dcf5";
//string mapFile = "map_10000.json";
string mapFile = "map50.json";

/*
///////////////////////////////////////////////////////////////////
//Here comes the meat.
///////////////////////////////////////////////////////////////////
*/


var serverUtilsLocal = new ServerUtils(gameUrlLocal, apiKey);
var serverUtilsRemote = new ServerUtils(gameUrlRemote, apiKey);

//Read the map with all the customers
MapData map = GameUtils.GetMap(mapFile);
if (!GameUtils.IsCustomerNamesUnique(map))
{
    //Do we actually need customer names to be unique?
    throw new Exception("Customer names are not unique. This was promised during training.");
}

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

var customerDetails = SimulatedAnnealingFacade.Run(serverUtilsLocal, map, personalities);

var selectedCustomers = CustomerSelector.Select(map, customerDetails);

var gameInput = LoanUtils.CreateGameInput(map.name, map.gameLengthInMonths, selectedCustomers);

Console.WriteLine("Final game input:");
var prettyJson = JsonConvert.SerializeObject(gameInput, Formatting.Indented);
File.WriteAllText("finalGameInput.json", prettyJson);
Console.WriteLine(prettyJson);

//Submit to remote server. 
var gameResponse = serverUtilsRemote.SubmitGameAsync(gameInput).Result;

Console.WriteLine("Final submission response:");
Console.WriteLine(gameResponse.ToString());


double totalScore = GameUtils.GetTotalScore(gameResponse);
Console.WriteLine("Final submission total score:");
Console.WriteLine(totalScore.ToString());

Console.WriteLine("Done.");

//TODO calculate how much budget we have left after granting all the loans. 
//TODO where does the final OptimalInterestRate get rounded to nice decial places? 
//TODO Extract docker image dlls to run scoring on native code. 
//TODO infer minAcceptedInterestRate, maxAcceptedInterestRate
//TODO maybe use TotalProfit instead of TotalScore
//TODO perhaps a genetic algorithm to explore awards. 

