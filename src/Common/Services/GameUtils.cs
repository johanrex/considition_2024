﻿//using Newtonsoft.Json;
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
        public static bool IsCustomerNamesUnique(Map map)
        {
            var customerNames = map.Customers.Select(c => c.Name);
            return customerNames.Distinct().Count() == customerNames.Count();
        }

        //TODO do I really need this function?
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
                var gameCustomerActionsCurrentIteration = new CustomerActionIteration();
                iterations.Add(gameCustomerActionsCurrentIteration);

                gameCustomerActionsCurrentIteration.CustomerActions = new Dictionary<string, CustomerAction>();

                //har jag någon för denna iterationen?
                foreach (CustomerLoanRequestProposalEx propositionDetail in propositionDetails)
                {
                    if (propositionDetail.Iterations == null) //Give the default NoAward if we don't have anything better. 
                    {
                        gameCustomerActionsCurrentIteration.CustomerActions[propositionDetail.CustomerName] = new CustomerAction
                        {
                            Type = CustomerActionType.Skip,
                            Award = AwardType.None
                        };
                    }
                    else //we got well thought out award data
                    {
                        if (propositionDetail.Iterations.Count > i)
                        {
                            Dictionary<string, CustomerAction> singleCustomerActions = propositionDetail.Iterations[i].CustomerActions;
                            if (singleCustomerActions == null)
                                throw new Exception("Iteration is null");

                            //should only be one customer, but let's loop through them anyway
                            foreach (var kvp in singleCustomerActions)
                            {
                                string propositionCustomerName = kvp.Key;
                                CustomerAction propositionAction = kvp.Value;

                                gameCustomerActionsCurrentIteration.CustomerActions[propositionCustomerName] = new CustomerAction
                                {
                                    Type = propositionAction.Type,
                                    Award = propositionAction.Award
                                };
                            }
                        }
                    }
                }
            }

            GameInput input = new()
            {
                MapName = mapName,
                Proposals = proposals,
                Iterations = iterations
            };

            return input;
        }


        public static GameInput CreateSingleCustomerGameInput(string mapName, int gameLengthInMonths, string customerName, double yearlyInterestRate, int monthsToPayBackLoan, List<CustomerActionIteration> iterations = null)
        {
            //Create proposal
            var proposal = CreateCustomerProposal(customerName, yearlyInterestRate, monthsToPayBackLoan, gameLengthInMonths);

            if (iterations == null)
            {
                //Create actions.
                iterations = new();

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
