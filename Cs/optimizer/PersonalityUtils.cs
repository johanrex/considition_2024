using Newtonsoft.Json;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;

namespace optimizer
{
    internal class PersonalityUtils
    {
        public static Personality StringToEnum(string personalityString)
        {
            if (!Enum.TryParse(personalityString, out Personality personalityEnum))
            {
                throw new Exception($"Can't find matching enum for personality {personalityString}.");
            }
            return personalityEnum;
        }

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

        public static bool HasKnownPersonalities(MapData map, Dictionary<Personality, PersonalitySpecification> personalities)
        {
            foreach (var customer in map.customers)
            {
                // Convert the string to the Personality enum
                if (!Enum.TryParse(customer.personality, out Personality personalityEnum))
                {
                    return false;
                }

                if (!personalities.ContainsKey(personalityEnum))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool HasKnownInterestRates(Dictionary<Personality, PersonalitySpecification> personalities)
        {
            foreach (var personality in personalities.Values)
            {
                if (personality.AcceptedMinInterest == null || personality.AcceptedMaxInterest == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
