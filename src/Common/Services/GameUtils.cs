//using Newtonsoft.Json;
//using optimizer.Models.Pocos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Models;

namespace Common.Services
{
    public class GameUtils
    {
        //public static Map GetMap(string mapFilename)
        //{
        //    var jsonSerializerOptions = new JsonSerializerOptions()
        //    {
        //        PropertyNameCaseInsensitive = true
        //    };

        //    Map map = JsonSerializer.Deserialize<Map>(File.ReadAllText(mapFilename), jsonSerializerOptions);

        //    return map;
        //}


        //public static Dictionary<AwardType, AwardSpecification> GetAwards(string awardsFilename)
        //{
        //    string json = File.ReadAllText(awardsFilename);

        //    var jsonSerializerOptions = new JsonSerializerOptions()
        //    {
        //        PropertyNameCaseInsensitive = true
        //    };

        //    Dictionary<AwardType, AwardSpecification> awards;

        //    using (JsonDocument document = JsonDocument.Parse(json))
        //    {
        //        JsonElement root = document.RootElement;
        //        JsonElement awardsElement = root.GetProperty("Awards");

        //        awards = JsonSerializer.Deserialize<Dictionary<AwardType, AwardSpecification>>(awardsElement.GetRawText(), jsonSerializerOptions);
        //    }

        //    return awards;
        //}



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

            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var responseJson = JsonSerializer.Serialize(gameResponse, gameResponse.GetType(), jsonSerializerOptions);
            
            File.WriteAllText(filename, responseJson);

            return totalScore;
        }

        public static GameInput CreateGameInput(string mapName, int gameLengthInMonths, List<CustomerLoanRequestProposalEx> propositionDetails)
        {
            var proposals = new List<CustomerLoanRequestProposal>();
            foreach (var proposition in propositionDetails)
            {
                var proposal = new CustomerLoanRequestProposal()
                {
                    CustomerName = proposition.CustomerName,
                    YearlyInterestRate = proposition.YearlyInterestRate,
                    MonthsToPayBackLoan = proposition.MonthsToPayBackLoan
                };

                proposals.Add(proposal);
            }

            List<CustomerActionIteration> iterations = new();

            for (int i = 0; i < gameLengthInMonths; i++)
            {
                CustomerActionIteration customerActionIteration = new();
                Dictionary<string, CustomerAction> custActions = new();
                customerActionIteration.CustomerActions = custActions;

                foreach (var proposition in propositionDetails)
                {
                    custActions[proposition.CustomerName] = new CustomerAction
                    {
                        Type = CustomerActionType.Skip,
                        Award = AwardType.None
                    };
                }

                iterations.Add(customerActionIteration);
            }

            GameInput input = new()
            {
                MapName = mapName,
                Proposals = proposals,
                Iterations = iterations
            };

            return input;
        }


        public static GameInput CreateSingleCustomerGameInput(string mapName, int gameLengthInMonths, string customerName, double yearlyInterestRate, int monthsToPayBackLoan)
        {
            //Create proposal
            var proposal = CreateCustomerProposal(customerName, yearlyInterestRate, monthsToPayBackLoan, gameLengthInMonths);

            //Create actions.
            List<CustomerActionIteration> iterations = new();

            for (int i = 0; i < gameLengthInMonths; i++)
            {
                CustomerActionIteration customerActionIteration = new();
                Dictionary<string, CustomerAction> custActions = new();
                customerActionIteration.CustomerActions = custActions;

                custActions[customerName] = new CustomerAction
                {
                    Type = CustomerActionType.Skip,
                    Award = AwardType.None
                };

                iterations.Add(customerActionIteration);
            }

            GameInput input = new()
            {
                MapName = mapName,
                Proposals = [proposal],
                Iterations = iterations
            };

            return input;
        }

        public static CustomerLoanRequestProposal CreateCustomerProposal(string customerName, double yearlyInterestRate, int monthsToPayBackLoan, int gameLengthInMonths)
        {
            var proposal = new CustomerLoanRequestProposal()
            {
                CustomerName = customerName,
                YearlyInterestRate = yearlyInterestRate,
                MonthsToPayBackLoan = monthsToPayBackLoan
            };

            return proposal;
        }
    }
}
