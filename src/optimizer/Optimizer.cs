using Newtonsoft.Json;
using optimizer;
using optimizer.Models.Pocos;
using optimizer.Strategies;

string gameUrlRemote = "https://api.considition.com/";
string gameUrlLocal = "http://localhost:8080/";

string apiKey = "05ae5782-1936-4c6a-870b-f3d64089dcf5";
//string mapFile = "map_10000.json";
string mapFile = "map.json";

/*
///////////////////////////////////////////////////////////////////
//Here comes the meat.
///////////////////////////////////////////////////////////////////
*/


var serverUtilsLocal = new ServerUtils(gameUrlLocal, apiKey);
var serverUtilsRemote = new ServerUtils(gameUrlRemote, apiKey);

//Read the map with all the customers
MapData map = GameUtils.GetMap(mapFile);

Console.WriteLine("Map name: " + map.name);

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
//var bruteForceDetails = new BruteForce().Run(serverUtilsLocal, map, personalities);

var selectedCustomers = CustomerSelector.Select(map, customerDetails);

var gameInput = LoanUtils.CreateGameInput(map.name, map.gameLengthInMonths, selectedCustomers);

//Log input 
Console.WriteLine("Final game input:");
var inputJson = JsonConvert.SerializeObject(gameInput, Formatting.Indented);
//Console.WriteLine(inputJson);
File.WriteAllText("finalGameInput.json", inputJson);

//Submit to remote server. 
var gameResponse = serverUtilsRemote.SubmitGameAsync(gameInput).Result;

//Log output
var responseJson = JsonConvert.SerializeObject(gameResponse, Formatting.Indented);
Console.WriteLine("Final submission response:");
Console.WriteLine(responseJson);
File.WriteAllText("finalGameOutput.json", responseJson);

double totalScore = GameUtils.GetTotalScore(gameResponse);
Console.WriteLine("Final submission total score:");
Console.WriteLine(totalScore.ToString());

//Paranoid check that if we submit to local server we should get the same result.
var gameResponseLocal = serverUtilsLocal.SubmitGameAsync(gameInput).Result;
double totalScore2 = GameUtils.GetTotalScore(gameResponse);

if (totalScore != totalScore2)
{
    throw new Exception("Local and remote server gave different scores.");
}

Console.WriteLine("Done.");


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
284599,9999999999   SA maxIterations: 1000, coolingRate: 0.95, initialTemp: 1000.0, maxMonthsToPayBackLoan = 50 * 12, Time: 18s, Customers processed per second: 0,27689783951679064 

*/
