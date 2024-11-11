using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Common.Services;

namespace optimizer.Strategies
{
    internal class SelectCustomersGreedy
    {
        public static List<CustomerLoanRequestProposalEx> Select(Dictionary<string, double> customerNameCosts, Map map, List<CustomerLoanRequestProposalEx> customerDetails)
        {
            Console.WriteLine("Selecting customers within budget: Greedy approach.");
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Sort customers by the ratio of ScoreContribution to LoanAmount in descending order
            var sortedCustomerDetails = customerDetails.OrderByDescending(c =>
            {
                var totalCustomerCost = customerNameCosts[c.CustomerName];
                return c.TotalScore / totalCustomerCost;
            }).ToList();

            //Select customers until we can't afford it anymore
            double startBudget = map.Budget;
            double moneySpent = 0;
            List<CustomerLoanRequestProposalEx> selectedCustomers = new List<CustomerLoanRequestProposalEx>();
            foreach (CustomerLoanRequestProposalEx customer in sortedCustomerDetails)
            {
                var totalCustomerCost = customerNameCosts[customer.CustomerName];

                if (moneySpent + totalCustomerCost <= startBudget)
                {
                    moneySpent += totalCustomerCost;
                    selectedCustomers.Add(customer);
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Greedy selection took {stopwatch.ElapsedMilliseconds} ms.");

            return selectedCustomers;
        }
    }
}


