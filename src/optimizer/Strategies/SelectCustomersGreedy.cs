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
        public static List<CustomerLoanRequestProposalEx> Select(ConfigService configService, Map map, List<CustomerLoanRequestProposalEx> customerDetails)
        {
            Console.WriteLine("Selecting customers within budget: Greedy approach.");
            Stopwatch stopwatch = Stopwatch.StartNew();

            var awardsSpecs = configService.GetAwardSpecifications(map.Name);

            //find out the total cost of each customer
            List<(CustomerLoanRequestProposalEx, double)> customerCosts = new();
            foreach (var customer in customerDetails)
            {
                double totalAwardCost = 0;
                foreach (CustomerActionIteration iteration in customer.Iterations)
                {
                    var customerAction = iteration.CustomerActions[customer.CustomerName];
                    if (customerAction.Award == AwardType.None)
                        continue;

                    AwardSpecification spec = awardsSpecs[customerAction.Award];
                    totalAwardCost += spec.Cost;
                }

                double totalCustomerCost = customer.LoanAmount + totalAwardCost;
                //double totalCustomerCost = customer.LoanAmount;
                (CustomerLoanRequestProposalEx, double) tpl = (customer, totalCustomerCost);

                customerCosts.Add(tpl);
            }

            // Sort customers by the ratio of ScoreContribution to LoanAmount in descending order
            var sortedCustomerCosts = customerCosts.OrderByDescending(tpl =>
            {
                var customer = tpl.Item1;
                var totalCustomerCost = tpl.Item2;
                return customer.TotalScore / totalCustomerCost;
            }).ToList();
            //var sortedCustomerCosts = customerCosts.OrderByDescending(tpl =>
            //{
            //    var customer = tpl.Item1;
            //    var totalCustomerCost = tpl.Item2;
            //    return customer.TotalScore / totalCustomerCost;
            //}).ToList();

            //Select customers until we can't afford it anymore
            double startBudget = map.Budget;
            double moneySpent = 0;
            List<CustomerLoanRequestProposalEx> selectedCustomers = new List<CustomerLoanRequestProposalEx>();
            foreach ((CustomerLoanRequestProposalEx,double) tpl in sortedCustomerCosts)
            {
                var customer = tpl.Item1;
                var totalCustomerCost = tpl.Item2;

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


