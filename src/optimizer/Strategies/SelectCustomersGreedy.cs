using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace optimizer.Strategies
{
    internal class SelectCustomersGreedy
    {
        public static List<CustomerLoanRequestProposalEx> Select(Map map, List<CustomerLoanRequestProposalEx> customerDetails)
        {
            Console.WriteLine("Selecting customers within budget: Greedy approach.");
            Stopwatch stopwatch = Stopwatch.StartNew();

            double budget = map.Budget;

            // Sort customers by the ratio of ScoreContribution to LoanAmount in descending order
            var sortedCustomers = customerDetails
                .OrderByDescending(c => c.TotalScore / c.Cost)
                .ToList();

            List<CustomerLoanRequestProposalEx> selectedCustomers = new List<CustomerLoanRequestProposalEx>();

            foreach (var customer in sortedCustomers)
            {
                if (customer.Cost <= budget)
                {
                    selectedCustomers.Add(customer);
                    budget -= customer.Cost;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Greedy selection took {stopwatch.ElapsedMilliseconds} ms.");

            return selectedCustomers;
        }
    }
}


