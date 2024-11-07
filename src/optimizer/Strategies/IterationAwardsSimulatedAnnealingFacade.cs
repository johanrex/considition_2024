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
            Map map, 
            List<CustomerLoanRequestProposalEx> proposalExs,
            ConfigService configService,
            Dictionary<string, Customer> mapCustomerLookup,
            Dictionary<Personality, PersonalitySpecification> personalities,
            Dictionary<AwardType, AwardSpecification> awards
            )
        {
            Console.WriteLine("Starting simulated annealing. Awards for single customer.");

            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            var bestProposalExs = new ConcurrentBag<CustomerLoanRequestProposalEx>();

            double temperature = 1.0;
            double coolingRate = 0.003;
            int maxIterations = 2000;
            var retries = 3;

            // Parallel for loop
            Parallel.For(0, proposalExs.Count, i =>
            {
                var proposalEx = proposalExs[i];
                IterationAwardsSimulatedAnnealing annealing = new IterationAwardsSimulatedAnnealing(
                    map,
                    proposalEx,
                    configService,
                    mapCustomerLookup,
                    personalities,
                    awards,
                    temperature,
                    coolingRate,
                    maxIterations
                    );

                var bestProposal = annealing.Run();
                bestProposalExs.Add(bestProposal);
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
