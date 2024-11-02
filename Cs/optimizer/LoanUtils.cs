using Microsoft.VisualBasic;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;

namespace optimizer
{
    internal class LoanUtils
    {
        public static GameInput CreateGameInput(string mapName, int gameLengthInMonths, List<CustomerPropositionDetails> propositionDetails)
        {
            var proposals = new List<CustomerLoanRequestProposal>();
            foreach (var proposition in propositionDetails)
            {
                var proposal = new CustomerLoanRequestProposal()
                {
                    CustomerName = proposition.CustomerName,
                    YearlyInterestRate = proposition.OptimalInterestRate,
                    MonthsToPayBackLoan = proposition.OptimalMonthsPayBack
                };

                proposals.Add(proposal);
            }

            var iterations = new List<Dictionary<string, CustomerAction>>();
            for (int i = 0; i < gameLengthInMonths; i++)
            {
                var custActions = new Dictionary<string, CustomerAction>();

                //TODO don't use the variable name customer, it's something else. Also above
                foreach (var proposition in propositionDetails)
                {
                    custActions[proposition.CustomerName] = new CustomerAction
                    {
                        Type = "Skip",
                        Award = "None"
                    };
                }

                iterations.Add(custActions);
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
            //Must create action for all customers during the whole game length 
            var custActions = new List<Dictionary<string, CustomerAction>>();
            for (int i = 0; i < gameLengthInMonths; i++)
            {
                var custAction = new Dictionary<string, CustomerAction>();
                custAction[customerName] = new CustomerAction
                {
                    Type = "Skip",
                    Award = "None"
                };

                custActions.Add(custAction);
            }

            GameInput input = new()
            {
                MapName = mapName,
                Proposals = [proposal],
                Iterations = custActions
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
