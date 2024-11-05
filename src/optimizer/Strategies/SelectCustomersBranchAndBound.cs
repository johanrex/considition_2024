using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common.Models;

namespace optimizer.Strategies
{
    internal class SelectCustomersBranchAndBound
    {
        public static List<CustomerPropositionDetails> Select(Map map, List<CustomerPropositionDetails> customerDetails)
        {
            Console.WriteLine("Selecting customers: Branch and Bound.");

            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            double budget = map.Budget;

            // Sort customers by the ratio of ScoreContribution to LoanAmount in descending order
            var sortedCustomers = customerDetails
                .OrderByDescending(c => c.ScoreContribution / c.LoanAmount)
                .ToList();

            // Initialize the best solution
            double maxScore = 0;
            List<CustomerPropositionDetails> bestSolution = new List<CustomerPropositionDetails>();

            // Create a priority queue for the nodes
            var pq = new PriorityQueue<Node, double>();

            // Add the root node
            pq.Enqueue(new Node(), 0);

            while (pq.Count > 0)
            {
                var node = pq.Dequeue();

                // If the node is promising
                if (node.Level < sortedCustomers.Count && node.Bound > maxScore)
                {
                    // Take the current item
                    var nextNode = new Node
                    {
                        Level = node.Level + 1,
                        Weight = node.Weight + sortedCustomers[node.Level].LoanAmount,
                        Score = node.Score + sortedCustomers[node.Level].ScoreContribution,
                        Items = new List<CustomerPropositionDetails>(node.Items)
                    };
                    nextNode.Items.Add(sortedCustomers[node.Level]);

                    // If the weight is within the budget and the score is better, update the best solution
                    if (nextNode.Weight <= budget && nextNode.Score > maxScore)
                    {
                        maxScore = nextNode.Score;
                        bestSolution = nextNode.Items;
                    }

                    // Calculate the bound for the next node
                    nextNode.Bound = CalculateBound(nextNode, sortedCustomers, budget);

                    // If the bound is promising, add the node to the queue
                    if (nextNode.Bound > maxScore)
                    {
                        pq.Enqueue(nextNode, -nextNode.Bound);
                    }

                    // Do not take the current item
                    var withoutCurrentNode = new Node
                    {
                        Level = node.Level + 1,
                        Weight = node.Weight,
                        Score = node.Score,
                        Items = new List<CustomerPropositionDetails>(node.Items)
                    };

                    // Calculate the bound for the next node
                    withoutCurrentNode.Bound = CalculateBound(withoutCurrentNode, sortedCustomers, budget);

                    // If the bound is promising, add the node to the queue
                    if (withoutCurrentNode.Bound > maxScore)
                    {
                        pq.Enqueue(withoutCurrentNode, -withoutCurrentNode.Bound);
                    }
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Branch and Bound selection took {stopwatch.ElapsedMilliseconds} ms.");

            return bestSolution;
        }

        private static double CalculateBound(Node node, List<CustomerPropositionDetails> customers, double budget)
        {
            if (node.Weight >= budget)
            {
                return 0;
            }

            double bound = node.Score;
            double totalWeight = node.Weight;
            int level = node.Level;

            while (level < customers.Count && totalWeight + customers[level].LoanAmount <= budget)
            {
                totalWeight += customers[level].LoanAmount;
                bound += customers[level].ScoreContribution;
                level++;
            }

            if (level < customers.Count)
            {
                bound += (budget - totalWeight) * (customers[level].ScoreContribution / customers[level].LoanAmount);
            }

            return bound;
        }

        private class Node
        {
            public int Level { get; set; }
            public double Weight { get; set; }
            public double Score { get; set; }
            public double Bound { get; set; }
            public List<CustomerPropositionDetails> Items { get; set; } = new List<CustomerPropositionDetails>();
        }
    }
}
