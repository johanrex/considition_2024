using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common.Models;

namespace optimizer.Strategies
{
    internal class IterationAwardsGreedy
    {
        public static void Award(Map map, List<CustomerLoanRequestProposalEx> customerDetails)
        {
            Console.WriteLine("Awarding customers: Greedy.");
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Calculate metrics for each customer
            var customerMetrics = customerDetails.Select(c => new
            {
                Customer = c,
                HappinessImpact = CalculateHappinessImpact(c),
                BankruptcyRisk = CalculateBankruptcyRisk(c),
                ProfitabilityImpact = CalculateProfitabilityImpact(c),
                LoanPaymentAbility = CalculateLoanPaymentAbility(c)
            }).ToList();

            // Sort customers by a combined score of metrics
            var sortedCustomers = customerMetrics
                .OrderByDescending(c => c.BankruptcyRisk * c.ProfitabilityImpact)
                .ThenByDescending(c => c.HappinessImpact)
                .ToList();

            // Allocate awards based on sorted list
            foreach (var customerMetric in sortedCustomers)
            {
                if (ShouldAward(customerMetric))
                {
                    GiveAward(customerMetric.Customer);
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Greedy awarding took {stopwatch.ElapsedMilliseconds} ms.");
        }

        private static double CalculateHappinessImpact(CustomerLoanRequestProposalEx customer)
        {
            // Implement logic to calculate the impact of an award on customer happiness
            return customer.TotalScore * 0.1; // Example calculation
        }

        private static double CalculateBankruptcyRisk(CustomerLoanRequestProposalEx customer)
        {
            // Implement logic to calculate the risk of bankruptcy without an award
            return customer.Cost > 1000 ? 0.5 : 0.1; // Example calculation
        }

        private static double CalculateProfitabilityImpact(CustomerLoanRequestProposalEx customer)
        {
            // Implement logic to calculate the impact of an award on profitability
            return customer.TotalScore * 0.2; // Example calculation
        }

        private static bool CalculateLoanPaymentAbility(CustomerLoanRequestProposalEx customer)
        {
            // Implement logic to determine if the customer can pay their loan without the award
            return customer.Cost < 500; // Example calculation
        }

        private static bool ShouldAward(dynamic customerMetric)
        {
            // Implement logic to decide if an award should be given based on metrics
            return customerMetric.BankruptcyRisk > 0.3 && customerMetric.LoanPaymentAbility == false;
        }

        private static void GiveAward(CustomerLoanRequestProposalEx customer)
        {
            // Implement logic to give an award to the customer
            Console.WriteLine($"Award given to {customer.CustomerName}");
        }
    }
}
