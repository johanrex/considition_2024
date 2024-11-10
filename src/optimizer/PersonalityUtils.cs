using Newtonsoft.Json;
using Common.Models;

using System.Collections.Generic;

namespace optimizer
{
    public class PersonalityUtils
    {
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
