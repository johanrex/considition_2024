using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace optimizer.Strategies
{
    internal class SelectCustomersDp
    {
        public static List<CustomerLoanRequestProposalEx> Select(Map map, List<CustomerLoanRequestProposalEx> customerDetails)
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
                for (int w = intBudget; w >= (int)customerDetails[i].Cost; w--)
                {
                    double newValue = dp[w - (int)customerDetails[i].Cost] + customerDetails[i].TotalScore;
                    if (newValue > dp[w])
                    {
                        dp[w] = newValue;
                        selected[w] = i;
                    }
                }
            }

            // Backtrack to find the selected items
            List<CustomerLoanRequestProposalEx> selectedCustomers = new List<CustomerLoanRequestProposalEx>();
            for (int w = intBudget; w > 0;)
            {
                int i = selected[w];
                if (dp[w] != dp[w - (int)customerDetails[i].Cost])
                {
                    selectedCustomers.Add(customerDetails[i]);
                    w -= (int)customerDetails[i].Cost;
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
