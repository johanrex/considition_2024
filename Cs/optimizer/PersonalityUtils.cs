using LocalHost.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    internal class PersonalityUtils
    {
        internal static Dictionary<string, PersonalitySpecification> GetHardcodedPersonalities()
        {
            // Read the JSON file
            string jsonFilePath = "personalities.json";
            string jsonData = File.ReadAllText(jsonFilePath);

            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, PersonalitySpecification>>>(jsonData);
            if (!jsonObject.ContainsKey("Personalities"))
            {
                throw new KeyNotFoundException("The key 'Personalities' was not found in the JSON data.");
            }

            var personalitiesJson = JsonConvert.SerializeObject(jsonObject["Personalities"]);
            var personalitiesDict = JsonConvert.DeserializeObject<Dictionary<string, PersonalitySpecification>>(personalitiesJson);
            return personalitiesDict;
        }
    }
}
