﻿using Newtonsoft.Json;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System.Collections.Generic;

namespace optimizer
{
    public class PersonalityUtils
    {
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

        /// <param name="personalitiesFile">Should be the file with known personalities known during training. It's used as the basis for inferring the competition personalities.</param>
        public static Dictionary<Personality, PersonalitySpecification> InferPersonalityInterestRatesBounds(ServerUtils serverUtils, Map map, string personalitiesFile)
        {
            var personalities = ReadPersonalitiesFile(personalitiesFile);

            //clean out the interest rates so we can infer them. 
            foreach (var personality in personalities.Values)
            {
                personality.AcceptedMinInterest = null;
                personality.AcceptedMaxInterest = null;
            }

            //Paranoid check
            var s1 = new HashSet<Personality>(map.Customers.Select(c => c.Personality).Distinct());
            var s2 = new HashSet<Personality>(personalities.Keys);
            if (!s1.SetEquals(s2))
            {
                throw new Exception("Mismatch between known personalities in the json file and the map.");
            }

            //Infer the interest rates
            foreach (var personalityEnum in personalities.Keys)
            {
                //Grab the first customer for this personality and pray that it's a good one. 
                var customer = map.Customers.First(c => c.Personality == personalityEnum);

                var startInterestRate = 0.01;
                const int monthsToPayBackLoan = 50;

                //Verify that the starting point produces a good output, meaning we are inside the accepted interval. 
                var gameInput = LoanUtils.CreateSingleCustomerGameInput(map.Name, map.GameLengthInMonths, customer.Name, startInterestRate, monthsToPayBackLoan);
                var gameResponse = serverUtils.SubmitGameAsync(gameInput).Result;
                var score = GameUtils.GetTotalScore(gameResponse);
                if (score<= 0)
                {
                    Console.WriteLine("Here's what we know so far:");
                    Console.WriteLine(JsonConvert.SerializeObject(personalities), Formatting.Indented); 
                    throw new Exception($"Bad score {score}. The starting interest rate ({startInterestRate}) was probably bad.");
                }

                //binary search for the max accepted interest rate
                int ubRequestCount = 0;
                double lowerBound = startInterestRate;
                double upperBound = 10.0;
                double maxAcceptedInterestRate = -1.0;
                while (upperBound - lowerBound > 0.001)
                {
                    double mid = (lowerBound + upperBound) / 2;
                    gameInput = LoanUtils.CreateSingleCustomerGameInput(map.Name, map.GameLengthInMonths, customer.Name, mid, monthsToPayBackLoan);
                    gameResponse = serverUtils.SubmitGameAsync(gameInput).Result;
                    score = GameUtils.GetTotalScore(gameResponse);
                    ubRequestCount++;
                    if (score > 0)
                    {
                        lowerBound = mid;
                        maxAcceptedInterestRate = mid;
                    }
                    else
                    {
                        upperBound = mid;
                    }
                }

                //binary search for the min accepted interest rate
                int lbRequestCount = 0;
                lowerBound = 0.0;
                upperBound = startInterestRate;
                double minAcceptedInterestRate = -1.0;
                while (upperBound - lowerBound > 0.001)
                {
                    double mid = (lowerBound + upperBound) / 2;
                    gameInput = LoanUtils.CreateSingleCustomerGameInput(map.Name, map.GameLengthInMonths, customer.Name, mid, monthsToPayBackLoan);
                    gameResponse = serverUtils.SubmitGameAsync(gameInput).Result;
                    score = GameUtils.GetTotalScore(gameResponse);
                    lbRequestCount++;
                    if (score > 0)
                    {
                        upperBound = mid;
                        minAcceptedInterestRate = mid;
                    }
                    else
                    {
                        lowerBound = mid;
                    }
                }

                if (minAcceptedInterestRate == -1.0 || maxAcceptedInterestRate == -1.0)
                {
                    throw new Exception("Couldn't find the min or max accepted interest rate.");
                }

                personalities[personalityEnum].AcceptedMinInterest = minAcceptedInterestRate;
                personalities[personalityEnum].AcceptedMaxInterest = maxAcceptedInterestRate;

                int i = 0;
            }

            return personalities;
        }
    }
}
