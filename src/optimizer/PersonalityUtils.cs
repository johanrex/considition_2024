using Newtonsoft.Json;
using Common.Models;

using System.Collections.Generic;

namespace optimizer
{
    public class PersonalityUtils
    {
        /*
        public static Personality StringToEnum(string personalityString)
        {
            if (!Enum.TryParse(personalityString, out Personality personalityEnum))
            {
                throw new Exception($"Can't find matching enum for personality {personalityString}.");
            }
            return personalityEnum;
        }

        public static Dictionary<Personality, PersonalitySpecification> ReadPersonalitiesFile(string personalitiesFile)
        {
            string jsonData = File.ReadAllText(personalitiesFile);

            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, PersonalitySpecification>>>(jsonData);
            if (!jsonObject.ContainsKey("Personalities"))
            {
                throw new KeyNotFoundException("The key 'Personalities' was not found in the JSON data.");
            }

            var personalitiesJson = JsonConvert.SerializeObject(jsonObject["Personalities"]);
            var personalitiesDict = JsonConvert.DeserializeObject<Dictionary<Personality, PersonalitySpecification>>(personalitiesJson);
            return personalitiesDict;
        }
        */

        public static bool HasKnownPersonalities(Map map, Dictionary<Personality, PersonalitySpecification> personalities)
        {
            foreach (var customer in map.Customers)
            {
                if (!personalities.ContainsKey(customer.Personality))
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
