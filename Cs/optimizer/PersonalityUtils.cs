using Newtonsoft.Json;
using optimizer.Models.Simulation;

namespace optimizer
{
    internal class PersonalityUtils
    {
        internal static Dictionary<Personality, PersonalitySpecification> GetHardcodedPersonalities()
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
            var personalitiesDict = JsonConvert.DeserializeObject<Dictionary<Personality, PersonalitySpecification>>(personalitiesJson);
            return personalitiesDict;
        }
    }
}
