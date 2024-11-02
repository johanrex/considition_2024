using Newtonsoft.Json;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    internal class GameUtils
    {
        public static void PrettyPrintJson(object obj)
        {
            string prettyJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Console.WriteLine(prettyJson);
        }

        public static MapData GetMap(string mapFilename)
        {
            string mapDataText = File.ReadAllText(mapFilename);
            var map = JsonConvert.DeserializeObject<MapData>(mapDataText);
            return map;
        }

        public static bool IsCustomerNamesUnique(MapData map)
        {
            var customerNames = map.customers.Select(c => c.name);
            return customerNames.Distinct().Count() == customerNames.Count();
        }

        public static double GetTotalScore(GameResponse gameResponse)
        {
            //TODO this might not always have a score if something bad happened.
            //Consider throwing an exception.
            return gameResponse.Score.TotalScore;
        }
    }
}
