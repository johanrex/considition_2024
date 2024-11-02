using Newtonsoft.Json;
using optimizer;
using optimizer.Models.Simulation;
using optimizer.Strategies;

string gameUrlRemote = "https://api.considition.com/";
string gameUrlLocal = "http://localhost:8080/";

string apiKey = "05ae5782-1936-4c6a-870b-f3d64089dcf5";
//string mapFile = "map_10000.json";
string mapFile = "Config/map.json";

/*
///////////////////////////////////////////////////////////////////
//Here comes the meat.
///////////////////////////////////////////////////////////////////
*/


var serverUtilsLocal = new ServerUtils(gameUrlLocal, apiKey);
var serverUtilsRemote = new ServerUtils(gameUrlRemote, apiKey);

//Read the map with all the customers
Map map = GameUtils.GetMap(mapFile);

Console.WriteLine("Map name: " + map.Name);
Console.WriteLine("Customer count: " + map.Customers.Count.ToString());

if (!GameUtils.IsCustomerNamesUnique(map))
    throw new Exception("Customer names are not unique. This was promised during training.");


//TODO infer personalities instead.
var personalities = PersonalityUtils.GetHardcodedPersonalities();
if (!PersonalityUtils.HasKnownPersonalities(map, personalities))
    throw new Exception("Map contains unknown personalities.");

if (!PersonalityUtils.HasKnownInterestRates(personalities))
    throw new Exception("Some personalities have unknown interest rates.");

var customerDetails = SimulatedAnnealingFacade.Run(serverUtilsLocal, map, personalities);
//var bruteForceDetails = new BruteForce().Run(serverUtilsLocal, map, personalities);

var selectedCustomers = CustomerSelector.Select(map, customerDetails);

Console.WriteLine("Customers selected: " + selectedCustomers.Count.ToString());

Console.WriteLine("These customers were selected:");
Console.WriteLine(DataFrameHelper.ToDataFrame(selectedCustomers).ToString());

Console.WriteLine("These customers were NOT selected:");
Console.WriteLine(DataFrameHelper.ToDataFrame(customerDetails.Except(selectedCustomers)).ToString());

Console.WriteLine("Predicted score from selection process: ");
Console.WriteLine(selectedCustomers.Sum(c => c.ScoreContribution));

var gameInput = LoanUtils.CreateGameInput(map.Name, map.GameLengthInMonths, selectedCustomers);

//Log input 
var inputJson = JsonConvert.SerializeObject(gameInput, Formatting.Indented);
File.WriteAllText("finalGameInput.json", inputJson);
//Console.WriteLine("Final game input:");
//Console.WriteLine(inputJson);

//Score the game locally.
var gameResponse = serverUtilsLocal.SubmitGameAsync(gameInput).Result;
var totalScore = GameUtils.LogGameResponse(gameResponse, "finalGameOutput.json");

//Score the game remotely.
var gameResponseRemote = serverUtilsRemote.SubmitGameAsync(gameInput).Result;
var totalScoreRemote = GameUtils.LogGameResponse(gameResponseRemote, "finalGameOutputRemote.json");

if (totalScore != totalScoreRemote)
{
    throw new Exception("Local and remote server gave different scores.");
}

Console.WriteLine("Done.");

//TODO find out if any of the proposals result in someone going bankrupt.
//TODO labba med Award.NoInterestRate 
//TODO inspect the scoring code for performance bottlenecks. Extract it if necessary. Ask copilot for help. 
//TODO calculate how much budget we have left after granting all the loans. So we know how much we can spend on awards.
//TODO where does the final OptimalInterestRate get rounded to nice decial places? 
//TODO Extract docker image dlls to run scoring on native code. 
//TODO infer minAcceptedInterestRate, maxAcceptedInterestRate
//TODO maybe use TotalProfit instead of TotalScore
//TODO perhaps a genetic algorithm to explore awards. 


/*
 * Changelog
Score               Action
After Simulation Map+Customer:
284599,9999999999   SA maxIterations: 1000, coolingRate: 0.95, initialTemp: 1000.0, maxMonthsToPayBackLoan = 50 * 12, Time: 31s, Customers processed per second: 0,15703386989419763

Baseline:
284599,9999999999   SA maxIterations: 1000, coolingRate: 0.95, initialTemp: 1000.0, maxMonthsToPayBackLoan = 50 * 12, Time: 18s, Customers processed per second: 0,27689783951679064 




*/
