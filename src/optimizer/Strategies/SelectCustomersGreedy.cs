using optimizer.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer.Strategies
{
    internal class SelectCustomersGreedy
    {
        public static List<CustomerPropositionDetails> Select(Map map, List<CustomerPropositionDetails> customerDetails)
        {
            Console.WriteLine("Selecting customers: Greedy.");

            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            double budget = map.Budget;

            // Sort customers by the ratio of ScoreContribution to LoanAmount in descending order
            var sortedCustomers = customerDetails
                .OrderByDescending(c => c.ScoreContribution / c.LoanAmount)
                .ToList();

            List<CustomerPropositionDetails> selectedCustomers = new List<CustomerPropositionDetails>();

            foreach (var customer in sortedCustomers)
            {
                if (customer.LoanAmount <= budget)
                {
                    selectedCustomers.Add(customer);
                    budget -= customer.LoanAmount;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Greedy selection took {stopwatch.ElapsedMilliseconds} ms.");

            return selectedCustomers;
        }
    }
}


