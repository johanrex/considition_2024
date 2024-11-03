using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimizer.Models.Simulation;

namespace optimizer.Strategies
{
    internal class SelectCustomersDpBackTrack
    {
        public static List<CustomerPropositionDetails> Select(Map map, List<CustomerPropositionDetails> customerDetails)
        {
            Console.WriteLine("Selecting customers with dp+backtrack.");
            //TODO how do we select the most profitable customers within our budget?
            //Is this a knapsack problem? Are there other approaches?

            /*
            CustomerPropositionDetails { CustomerName = Glenn, ScoreContribution = -11100, LoanAmount = 300000,00, OptimalInterestRate = 50007,7542410451, OptimalMonthsPayBack = 31 }
            CustomerPropositionDetails { CustomerName = Emil, ScoreContribution = 2074,999999999999, LoanAmount = 20000,00, OptimalInterestRate = 0,05, OptimalMonthsPayBack = 15 }
            CustomerPropositionDetails { CustomerName = Kim, ScoreContribution = 500,0000000000001, LoanAmount = 2000,00, OptimalInterestRate = 0,1, OptimalMonthsPayBack = 51 }
            CustomerPropositionDetails { CustomerName = Gordon, ScoreContribution = 32024,999999999993, LoanAmount = 800000,00, OptimalInterestRate = 0,02, OptimalMonthsPayBack = 22 }
            CustomerPropositionDetails { CustomerName = Ada, ScoreContribution = 249999,9999999999, LoanAmount = 250000,00, OptimalInterestRate = 0,5, OptimalMonthsPayBack = 21 }
            */

            int n = customerDetails.Count;
            double budget = map.Budget;

            // Create a DP table
            double[,] dp = new double[n + 1, (int)budget + 1];

            // Fill the DP table
            for (int i = 1; i <= n; i++)
            {
                for (int w = 0; w <= budget; w++)
                {
                    if (customerDetails[i - 1].LoanAmount <= w)
                    {
                        dp[i, w] = Math.Max(dp[i - 1, w], dp[i - 1, w - (int)customerDetails[i - 1].LoanAmount] + customerDetails[i - 1].ScoreContribution);
                    }
                    else
                    {
                        dp[i, w] = dp[i - 1, w];
                    }
                }
            }

            // Backtrack to find the selected items
            List<CustomerPropositionDetails> selectedCustomers = new List<CustomerPropositionDetails>();
            for (int i = n, w = (int)budget; i > 0 && w >= 0; i--)
            {
                if (dp[i, w] != dp[i - 1, w])
                {
                    selectedCustomers.Add(customerDetails[i - 1]);
                    w -= (int)customerDetails[i - 1].LoanAmount;
                }
            }

            return selectedCustomers;
        }
    }
}
