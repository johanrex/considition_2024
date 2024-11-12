using Common.Models;
using Common.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer.Strategies
{
    internal class IterationAwardsSimulatedAnnealingFacade
    {
        public static List<CustomerLoanRequestProposalEx> Run(
            ScoreUtils scoreUtils,
            Map map, 
            List<CustomerLoanRequestProposalEx> proposalExs,
            int maxDegreeOfParallelism)
        {
            Console.WriteLine("Awards for single customer. Starting simulated annealing.");

            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            var bestProposalExs = new ConcurrentBag<CustomerLoanRequestProposalEx>();

            double temperature = 1.0;
            double coolingRate = 0.003;
            int maxIterations = 2000;
            var retries = 3;

            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = maxDegreeOfParallelism;

            // Parallel for loop
            Parallel.For(0, proposalExs.Count, options, i =>
            {
                CustomerLoanRequestProposalEx bestProposal = null;
                for (int _=0;_<retries; _++)
                {
                    // Let's test simulated annealing
                    // Get the proposalEx (CustomerLoanRequestProposalEx)
                    var proposalEx = proposalExs[i];
                    IterationAwardsSimulatedAnnealing annealing = new IterationAwardsSimulatedAnnealing(
                        scoreUtils,
                        map,
                        proposalEx,
                        temperature,
                        coolingRate,
                        maxIterations
                        );

                    var currentProposal = annealing.Run();
                    if (bestProposal == null || currentProposal.TotalScore > bestProposal.TotalScore)
                    {
                        bestProposal = currentProposal;
                    }
                }

                bestProposalExs.Add(bestProposal);

                //Log progress
                string msg = $"Determining awards for single customer ({bestProposalExs.Count}/{map.Customers.Count})";
                Console.WriteLine(msg);
            });

            // Stop the stopwatch
            stopwatch.Stop();

            // Calculate total time and customers per second
            double totalTimeInSeconds = stopwatch.Elapsed.TotalSeconds;
            double customersPerSecond = map.Customers.Count / totalTimeInSeconds;

            Console.WriteLine($"coolingRate: {coolingRate}");
            Console.WriteLine($"maxIterations: {maxIterations}");
            Console.WriteLine($"Simulated annealing total time taken: {totalTimeInSeconds} seconds");
            Console.WriteLine($"Customers processed per second: {customersPerSecond}");

            return bestProposalExs.ToList();
        }
    }
}
