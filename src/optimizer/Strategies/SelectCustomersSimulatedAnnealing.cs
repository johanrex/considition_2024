using Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace optimizer.Strategies
{
    internal class SelectCustomersSimulatedAnnealing
    {
        public static List<CustomerLoanRequestProposalEx> Select(Map map, List<CustomerLoanRequestProposalEx> customerDetails)
        {
            Console.WriteLine("Selecting customers within budget: Simulated Annealing approach.");
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
            Console.WriteLine($"Selection took {stopwatch.ElapsedMilliseconds} ms.");

            return selectedCustomers;
        }

        private double ScoreFunction(List<CustomerLoanRequestProposalEx> solution)
        {
            return solution.Sum(x => x.Value);
        }

    }
}
*/