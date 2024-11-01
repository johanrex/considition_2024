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

SimulatedAnnealingFacade.Run(gameUtils, map, personalities);



Console.WriteLine("Done.");