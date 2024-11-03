using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimizer.Models.Simulation;

namespace optimizer.Strategies
{
    internal class SelectCustomersDp
    {
        public static List<CustomerPropositionDetails> Select(Map map, List<CustomerPropositionDetails> customerDetails)
        {
            Console.WriteLine("Selecting customers: Dynamic Programming.");

            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            int n = customerDetails.Count;
            double budget = map.Budget;
            int intBudget = (int)budget;

            // Create a DP array
            double[] dp = new double[intBudget + 1];
            int[] selected = new int[intBudget + 1];

            // Fill the DP array
            for (int i = 0; i < n; i++)
            {
                for (int w = intBudget; w >= (int)customerDetails[i].LoanAmount; w--)
                {
                    double newValue = dp[w - (int)customerDetails[i].LoanAmount] + customerDetails[i].ScoreContribution;
                    if (newValue > dp[w])
                    {
                        dp[w] = newValue;
                        selected[w] = i;
                    }
                }
            }

            // Backtrack to find the selected items
            List<CustomerPropositionDetails> selectedCustomers = new List<CustomerPropositionDetails>();
            for (int w = intBudget; w > 0;)
            {
                int i = selected[w];
                if (dp[w] != dp[w - (int)customerDetails[i].LoanAmount])
                {
                    selectedCustomers.Add(customerDetails[i]);
                    w -= (int)customerDetails[i].LoanAmount;
                }
                else
                {
                    break;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Dynamic Programming selection took {stopwatch.ElapsedMilliseconds} ms.");

            return selectedCustomers;
        }
    }
}
