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
using optimizer.Models.Pocos;

namespace optimizer
{
    internal class GameUtils
    {
        public static Map GetMap(string mapFilename)
        {
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            Map map = JsonSerializer.Deserialize<Map>(File.ReadAllText(mapFilename), jsonSerializerOptions);

            return map;
        }


        public static Dictionary<AwardType, AwardSpecification> GetAwards(string awardsFilename)
        {
            string json = File.ReadAllText(awardsFilename);

            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            Dictionary<AwardType, AwardSpecification> awards;

            using (JsonDocument document = JsonDocument.Parse(json))
            {
                JsonElement root = document.RootElement;
                JsonElement awardsElement = root.GetProperty("Awards");

                awards = JsonSerializer.Deserialize<Dictionary<AwardType, AwardSpecification>>(awardsElement.GetRawText(), jsonSerializerOptions);
            }

            return awards;
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
