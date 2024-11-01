using Microsoft.VisualBasic;
using optimizer.Models.Pocos;
using optimizer.Models.Simulation;
using System;

namespace optimizer
{
    internal class LoanUtils
    {

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
