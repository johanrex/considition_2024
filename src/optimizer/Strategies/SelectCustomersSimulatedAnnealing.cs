using Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Services;

namespace optimizer.Strategies
{
    internal class SelectCustomersSimulatedAnnealing
    {
        public static List<CustomerLoanRequestProposalEx> Select(Dictionary<string, CustomerLoanRequestProposalEx> allCustomers, Dictionary<string, double> customerNameCosts, Map map)
        {
            Console.WriteLine("Selecting customers within budget: Simulated Annealing approach.");
            Stopwatch stopwatch = Stopwatch.StartNew();

            double budget = map.Budget;
            double initialTemperature = 100000;
            double coolingRate = 0.003;
            int retries = 3;

            // Get initial state
            List<CustomerLoanRequestProposalEx> initialState = GetInitialState(customerNameCosts, budget, allCustomers.Values.ToList());
            List<CustomerLoanRequestProposalEx> globalBestState = new List<CustomerLoanRequestProposalEx>(initialState);
            double globalBestScore = int.MinValue;


            Random random = new Random();
            int iterations = 0;
            
            for (int _=0;_<retries;_++)
            {
                List<CustomerLoanRequestProposalEx> currentState = new List<CustomerLoanRequestProposalEx>(initialState);
                List<CustomerLoanRequestProposalEx> bestState = new List<CustomerLoanRequestProposalEx>(initialState);
                double currentScore = ScoreFunction(currentState);
                double bestScore = currentScore;

                double temperature = initialTemperature;

                while (temperature > 1)
                {
                    List<CustomerLoanRequestProposalEx> neighbor = GetNeighbor(allCustomers, customerNameCosts, map, currentState);
                    double neighborScore = ScoreFunction(neighbor);

                    if (AcceptanceProbability(currentScore, neighborScore, temperature) > random.NextDouble())
                    {
                        currentState = neighbor;
                        currentScore = neighborScore;
                    }

                    if (currentScore > bestScore)
                    {
                        bestState = new List<CustomerLoanRequestProposalEx>(currentState);
                        bestScore = currentScore;
                    }

                    temperature *= 1 - coolingRate;
                    iterations++;
                }

                if (bestScore>globalBestScore)
                {
                    globalBestState = new List<CustomerLoanRequestProposalEx>(bestState);
                    globalBestScore = bestScore;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Selection took {stopwatch.ElapsedMilliseconds} ms.");

            return globalBestState;
        }

        private static List<CustomerLoanRequestProposalEx> GetInitialState(Dictionary<string, double> customerNameCosts, double budget, List<CustomerLoanRequestProposalEx> customerDetails)
        {
            //Select customers until we can't afford it anymore
            double moneySpent = 0;
            List<CustomerLoanRequestProposalEx> selectedCustomers = new List<CustomerLoanRequestProposalEx>();
            foreach (CustomerLoanRequestProposalEx customer in customerDetails)
            {
                var totalCustomerCost = customerNameCosts[customer.CustomerName];

                //can we add the current customer?
                if (moneySpent + totalCustomerCost > budget)
                    break;
                
                moneySpent += totalCustomerCost;
                selectedCustomers.Add(customer);
            }

            return selectedCustomers;
        }

        private static List<CustomerLoanRequestProposalEx> GetNeighbor(Dictionary<string, CustomerLoanRequestProposalEx> allCustomers, Dictionary<string, double> customerNameCosts, Map map, List<CustomerLoanRequestProposalEx> currentState)
        {
            Random random = new Random();

            //Make copy of current state
            List<CustomerLoanRequestProposalEx> neighbor = new(currentState);

            double budget = map.Budget;

            // Decide whether to add or remove a customer
            bool addCustomer = random.NextDouble() > 0.5;

            if (addCustomer)
            {
                // Find new customer to add that fits in the budget. 

                //TODO performance: O(n) loop
                List<string> neighborNames = neighbor.Select(c => c.CustomerName).ToList();
                List<string> notUsedNames = allCustomers.Keys.Except(neighborNames).ToList();

                //TODO performance: O(n) loop
                double totalNeighborCost = neighbor.Sum(c => customerNameCosts[c.CustomerName]);
                var notUsedNamesWithinBudget = notUsedNames.Where(name => totalNeighborCost + customerNameCosts[name] <= budget).ToList();

                if (notUsedNamesWithinBudget.Count > 0)
                {
                    var newCustomerName = notUsedNamesWithinBudget[random.Next(notUsedNamesWithinBudget.Count)];
                    var customer = allCustomers[newCustomerName];
                    neighbor.Add(customer);
                }
            }
            else
            {
                // Remove an existing customer
                if (neighbor.Count > 0)
                {
                    neighbor.RemoveAt(random.Next(neighbor.Count));
                }
            }

            return neighbor;
        }

        private static double AcceptanceProbability(double currentScore, double neighborScore, double temperature)
        {
            if (neighborScore > currentScore)
            {
                return 1.0;
            }
            return Math.Exp((neighborScore - currentScore) / temperature);
        }

        private static double ScoreFunction(List<CustomerLoanRequestProposalEx> solution)
        {
            return solution.Sum(x => x.TotalScore);
        }

    }
}
