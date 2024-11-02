//using Newtonsoft.Json;
//using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using optimizer.Models.Simulation;

namespace optimizer
{
    internal class GameUtils
    {
        public static Map GetMap(string mapFilename)
        {
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            Map map = JsonSerializer.Deserialize<Map>(File.ReadAllText(mapFilename), jsonSerializerOptions);

            return map;
        }

        public static bool IsCustomerNamesUnique(Map map)
        {
            var customerNames = map.Customers.Select(c => c.Name);
            return customerNames.Distinct().Count() == customerNames.Count();
        }

        public static double GetTotalScore(GameResponse gameResponse)
        {
            //TODO this might not always have a score if something bad happened.
            //Consider throwing an exception.
            return gameResponse.Score.TotalScore;
        }

        public static double LogGameResponse(GameResponse gameResponse, string filename)
        {
            double totalScore = GetTotalScore(gameResponse);
            Console.WriteLine("Game response total score:");
            Console.WriteLine(totalScore.ToString());

            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var responseJson = JsonSerializer.Serialize(gameResponse, gameResponse.GetType(), jsonSerializerOptions);
            
            File.WriteAllText(filename, responseJson);

            return totalScore;
        }
    }
}
